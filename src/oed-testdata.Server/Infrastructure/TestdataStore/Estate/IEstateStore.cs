namespace oed_testdata.Server.Infrastructure.TestdataStore.Estate;

public interface IEstateStore
{
    public Task<IEnumerable<EstateData>> ListAll();
    public Task<EstateData?> GetByEstateSsn(string estateSsn);
}