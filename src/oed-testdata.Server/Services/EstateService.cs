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

        var estateInstances = await altinnClient.GetOedInstancesByDeceasedNin(estate.EstateSsn) ?? [];

        // Delete the heirs' individual declarations (dd-private-probate) for this estate. They are owned by
        // the heir and point back through referrerOedInstanceId. Match on the deceased's party rather than
        // the current oed instance, so declarations left behind by earlier resets are removed too.
        var deceasedPartyIds = estateInstances
            .Concat(declarationInstances ?? [])
            .Select(instance => instance.InstanceOwner.PartyId)
            .ToHashSet();

        if (deceasedPartyIds.Count > 0)
        {
            foreach (var instanceOwner in GetHeirInstanceOwnerIdentifiers(estate.Data.DaCaseList.Single()))
            {
                var probateInstances = await altinnClient.GetDdPrivateProbateInstancesByInstanceOwner(instanceOwner);
                foreach (var probateInstance in probateInstances ?? [])
                {
                    if (probateInstance.DataValues?.TryGetValue("referrerOedInstanceId", out var referrer) != true ||
                        referrer is null ||
                        !deceasedPartyIds.Contains(referrer.Split("/").First()))
                    {
                        continue;
                    }

                    var partyId = probateInstance.InstanceOwner.PartyId;
                    var instanceGuid = probateInstance.Id.Split("/").Last();
                    await oedClient.DeleteDdPrivateProbateInstance(partyId, instanceGuid);
                }
            }
        }

        // Delete any existing instance data for this estate
        foreach (var estateInstance in estateInstances)
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

    private static IEnumerable<string> GetHeirInstanceOwnerIdentifiers(DaCase daCase)
    {
        foreach (var part in daCase.Parter)
        {
            switch (part)
            {
                case PersonPart { Nin: { Length: > 0 } nin }:
                    yield return $"person:{nin}";
                    break;
                // Storage only accepts the British spelling; "organization:" is rejected with 400
                case ForetakPart { OrganisasjonsNummer: { } orgNr }:
                    yield return $"organisation:{(long)orgNr}";
                    break;
            }
        }
    }
}
