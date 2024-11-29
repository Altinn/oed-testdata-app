namespace oed_testdata.Server.Infrastructure.TestdataStore;

public interface ITestdataStore
{
    public Task<IEnumerable<DaData>> ListAll();
    public Task<DaData> GetByEstateSsn(string estateSsn);
}