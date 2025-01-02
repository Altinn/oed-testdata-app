namespace oed_testdata.Server.Infrastructure.TestdataStore.Kartverket;

public interface IKartverketStore
{
    public Task<KartverketResponse> GetProperties(int partyId);
}