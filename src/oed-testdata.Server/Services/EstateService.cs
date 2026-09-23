using oed_testdata.Server.Infrastructure.Altinn;
using oed_testdata.Server.Infrastructure.OedEvents;
using oed_testdata.Server.Infrastructure.TestdataStore.Estate;

namespace oed_testdata.Server.Services;

public interface IEstateService
{
    /// <summary>
    /// Deletes any existing declaration and estate instances for the estate and posts a new DA event
    /// to create the estate from scratch. Returns null if the estate does not exist in the testdata store.
    /// </summary>
    public Task<EstateData?> CreateOrRecreateEstate(string estateSsn, DateTimeOffset? tilgangsdatoDigitaltDodsbo);
}

public class EstateService(
    IEstateStore store,
    IOedClient oedClient,
    IAltinnClient altinnClient)
    : IEstateService
{
    public async Task<EstateData?> CreateOrRecreateEstate(string estateSsn, DateTimeOffset? tilgangsdatoDigitaltDodsbo)
    {
        // Get estate from testapp store (files)
        var estate = await store.GetByEstateSsn(estateSsn);
        if (estate is null)
        {
            return null;
        }

        // Delete any existing declarations for this estate
        var declarationInstances = await altinnClient.GetOedDeclarationInstancesByDeceasedNin(estate.EstateSsn);
        foreach (var declarationInstance in declarationInstances ?? [])
        {
            var partyId = declarationInstance.InstanceOwner.PartyId;
            var declarationInstanceGuid = declarationInstance.Id.Split("/").Last();
            await oedClient.DeleteOedDeclarationInstance(partyId, declarationInstanceGuid);
        }

        // Delete any existing instance data for this estate
        var estateInstances = await altinnClient.GetOedInstancesByDeceasedNin(estate.EstateSsn);
        foreach (var estateInstance in estateInstances ?? [])
        {
            var partyId = estateInstance.InstanceOwner.PartyId;
            var instanceGuid = estateInstance.Id.Split("/").Last();
            await oedClient.DeleteOedInstance(partyId, instanceGuid);
        }

        // Update estate data and post DA event to create/recreate estate from scratch
        var data = estate.Data;
        data.SetMottattStatus();
        data.UpdateTimestamps(DateTimeOffset.Now);
        data.UpdateTilgangsdato(tilgangsdatoDigitaltDodsbo);

        await oedClient.PostDaEvent(data);

        return estate;
    }
}
