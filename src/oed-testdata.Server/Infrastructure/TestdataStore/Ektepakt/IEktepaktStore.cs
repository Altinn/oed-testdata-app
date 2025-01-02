using oed_testdata.Server.Infrastructure.TestdataStore.Kartverket;

namespace oed_testdata.Server.Infrastructure.TestdataStore.Ektepakt;

public interface IEktepaktStore
{
    public Task<EktepaktResponse> GetEktepakter(int partyId);
}