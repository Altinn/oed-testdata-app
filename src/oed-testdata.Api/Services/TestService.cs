using oed_testdata.Api.Infrastructure.Altinn;
using oed_testdata.Api.Infrastructure.TestdataStore;
using oed_testdata.Api.Models;

namespace oed_testdata.Api.Services
{
    public interface ITestService
    {
        public Task Test();
    }
    public class TestService(IAltinnClient altinnClient, ITestdataStore store) : ITestService
    {
        public async Task Test()
        {
            //var allEstates = await store.ListAll();
            //var estate = await store.GetByEstateSsn("24817296595");

            var instances = await altinnClient.GetOedInstancesByDeceasedNin("24817296595");


            var instanceId = instances.First().Id;
            var instanceDataId = instances.First().Data.First().Id;

            var data = await altinnClient.GetOedInstanceData<OED_M>(instanceId, instanceDataId);

        }
    }
}
