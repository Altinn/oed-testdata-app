using oed_testdata.Server.Infrastructure.Altinn;
using oed_testdata.Server.Infrastructure.TestdataStore;
using oed_testdata.Server.Models;

namespace oed_testdata.Server.Services
{
    public interface ITestService
    {
        public Task<OED_M> Test();
    }
    public class TestService(IAltinnClient altinnClient, ITestdataStore store) : ITestService
    {
        public async Task<OED_M> Test()
        {
            var instances = await altinnClient.GetOedInstancesByDeceasedNin("24817296595");

            var instanceId = instances.First().Id;
            var instanceDataId = instances.First().Data.First().Id;

            var data = await altinnClient.GetOedInstanceData<OED_M>(instanceId, instanceDataId);

            return data;
        }
    }
}
