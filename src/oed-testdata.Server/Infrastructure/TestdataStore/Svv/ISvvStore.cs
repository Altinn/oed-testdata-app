using oed_testdata.Server.Infrastructure.TestdataStore.Bank;

namespace oed_testdata.Server.Infrastructure.TestdataStore.Svv
{
    public interface ISvvStore
    {
        public Task<SvvResponse> GetVehicles(int partyId);
    }
}
