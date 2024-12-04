using Altinn.App.Models;
using oed_testdata.Server.Infrastructure.Altinn;
using oed_testdata.Server.Infrastructure.TestdataStore;
using oed_testdata.Server.Models;

namespace oed_testdata.Server.Services
{
    public interface ITestService
    {
        public Task<object> Test();
    }
    public class TestService(IAltinnClient altinnClient, ITestdataStore store) : ITestService
    {
        public async Task<object> Test()
        {
            var deceased = "24817296595";

            var instances = await altinnClient.GetOedInstancesByDeceasedNin(deceased);
            var data = await altinnClient.GetOedInstanceData<OED_M>(instances.First().Id, instances.First().Data.First().Id);

            var decalrationInstances = await altinnClient.GetOedDeclarationInstancesByDeceasedNin(deceased);
            var declarationData = await altinnClient.GetOedInstanceData<declaration>(decalrationInstances.First().Id, decalrationInstances.First().Data.First().Id);


            return declarationData;
        }
    }
}
