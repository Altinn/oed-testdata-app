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
        if (declarationInstances is { Count: > 0 })
        {
            var partyId = declarationInstances.First().InstanceOwner.PartyId;
            //var declarationInstanceGuid = declarationInstances.First().Data.First().InstanceGuid;
            var declarationInstanceGuid = declarationInstances.First().Id.Split("/")[1];
            await oedClient.DeleteOedDeclarationInstance(partyId, declarationInstanceGuid);
        }

        // Delete any existing instance data for this estate
        var estateInstances = await altinnClient.GetOedInstancesByDeceasedNin(estate.EstateSsn);
        if (estateInstances is { Count: > 0 })
        {
            var activeInstance = estateInstances.First();
            var partyId = activeInstance.InstanceOwner.PartyId;
            var instanceGuid = activeInstance.Id.Split("/").Last();

            await oedClient.DeleteOedInstance(partyId, instanceGuid);

            if (activeInstance.Data is { Count: > 0 })
            {
                var dataInstanceGuid = activeInstance.Data.First().InstanceGuid;
                await oedClient.DeleteOedInstance(partyId, dataInstanceGuid);
            }
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
