namespace oed_testdata.Server.Infrastructure.TestdataStore;

public interface ITestdataStore
{
    public Task<IEnumerable<EstateData>> ListAll();
    public Task<EstateData?> GetByEstateSsn(string estateSsn);
}