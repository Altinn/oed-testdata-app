using System.Text.Json;
using Altinn.App.Models;
using oed_testdata.Server.Infrastructure.Altinn;
using oed_testdata.Server.Infrastructure.Maskinporten;
using oed_testdata.Server.Infrastructure.Maskinporten.Models;
using oed_testdata.Server.Infrastructure.OedEvents;
using oed_testdata.Server.Infrastructure.TestdataStore;
using oed_testdata.Server.Models;
using static System.Net.WebRequestMethods;

namespace oed_testdata.Server.Services
{
    public interface ITestService
    {
        public Task<object> Test();
    }

    public class TestService(
        IAltinnClient altinnClient,
        IOedClient oedClient,
        ITestdataStore store,
        IHttpClientFactory httpClientFactory,
        IMaskinportenClient maskinportenClient)
        : ITestService
    {
        public async Task<object> Test()
        {
            //var deceased = "24817296595";
            var deceased = "18855699938";


            // Oed instance
            var oedInstances = await altinnClient.GetOedInstancesByDeceasedNin(deceased);

            if (oedInstances.Count == 0)
                return "No oed instance";

            var partyId = oedInstances.First().InstanceOwner.PartyId;
            var oedInstanceGuid = oedInstances.First().Data.First().InstanceGuid;
            var oedInstanceDataGuid = oedInstances.First().Data.First(data => data.ContentType == "application/xml").Id;

            // SLETTER
            //await oedClient.DeleteOedInstance(partyId, oedInstanceGuid);

            var oedInstanceData = await altinnClient.GetInstanceData<OED_M>(partyId, oedInstanceGuid, oedInstanceDataGuid);
            
            // OedDeclaration instance
            var decalrationInstances = await altinnClient.GetOedDeclarationInstancesByDeceasedNin(deceased);

            if (decalrationInstances.Count == 0)
                return oedInstanceData;

            var oedDeclarationInstanceGuid = decalrationInstances.First().Data.First().InstanceGuid;
            var oedDeclarationInstanceDataGuid = decalrationInstances.First().Data.First(data => data.ContentType == "application/xml").Id;

            var oedDeclarationInstanceData = await altinnClient.GetInstanceData<declaration>(
                partyId, 
                oedDeclarationInstanceGuid, 
                oedDeclarationInstanceDataGuid);

            // SLETTER
            //await oedClient.DeleteOedDeclarationInstance(partyId, oedDeclarationInstanceGuid);


            // Get declaration
            var declaration = await maskinportenClient.GetDeclaration(partyId, oedDeclarationInstanceGuid);


            //var maskinportenClient = httpClientFactory.CreateClient(MaskinportenConstants.HttpClientName);
            //var path = $"https://digdir.apps.tt02.altinn.no/digdir/oed/api/declarations/{partyId}/{oedDeclarationInstanceGuid}";
            //var response = await maskinportenClient.GetAsync(path);

            //var s = await response.Content.ReadAsStringAsync();
            ////await using var contentStream = await response.Content.ReadAsStreamAsync();
            //var data = JsonSerializer.Deserialize<Declaration>(s);

            ////var data = MaskinportenJsonSerializer.Deserialize<Declaration>(contentStream);
           

            return declaration;
        }
    }
}
