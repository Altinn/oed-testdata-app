namespace oed_testdata.Server.Infrastructure.TestdataStore;

public interface IBankStore
{
    public Task<IEnumerable<BankCustomerRelation>> GetCustomerRelations(int partyId);
    public Task<BankResponse> GetBankDetails(int partyId, string bankOrgNo);

    public Task<byte[]> GetAccountTransactionsFile(int partyId, string bankOrgNo, string accountRef);
}