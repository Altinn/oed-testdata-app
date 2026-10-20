using oed_testdata.Server.Infrastructure.Altinn;
using oed_testdata.Server.Infrastructure.Maskinporten;
using oed_testdata.Server.Models;

namespace oed_testdata.Server.Services;

public interface ITestService
{
    public Task<object> Test();
}

public class TestService(
    IAltinnClient altinnClient,
    IMaskinportenClient maskinportenClient)
    : ITestService
{
    public async Task<object> Test()
    {
        var deceased = "18855699938";


        // Oed instance
        var oedInstances = await altinnClient.GetOedInstancesByDeceasedNin(deceased);

        if (oedInstances.Count == 0)
            return "No oed instance";

        var partyId = oedInstances[0].InstanceOwner.PartyId;
        var oedInstanceGuid = oedInstances[0].Data[0].InstanceGuid;
        var oedInstanceDataGuid = oedInstances[0].Data.First(data => data.ContentType == "application/xml").Id;

        var oedInstanceData = await altinnClient.GetInstanceData<OED_M>(partyId, oedInstanceGuid, oedInstanceDataGuid);
            
        // OedDeclaration instance
        var decalrationInstances = await altinnClient.GetOedDeclarationInstancesByDeceasedNin(deceased);

        if (decalrationInstances.Count == 0)
            return oedInstanceData;

        var oedDeclarationInstanceGuid = decalrationInstances[0].Data[0].InstanceGuid;

        // Get declaration
        var declaration = await maskinportenClient.GetDeclaration(partyId, oedDeclarationInstanceGuid);


        return declaration;
    }
}