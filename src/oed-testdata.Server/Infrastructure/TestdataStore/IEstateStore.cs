namespace oed_testdata.Server.Infrastructure.TestdataStore;

public interface IEstateStore
{
    public Task<IEnumerable<EstateData>> ListAll();
    public Task<EstateData?> GetByEstateSsn(string estateSsn);
}