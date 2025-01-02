namespace oed_testdata.Server.Infrastructure.TestdataStore;

public interface ITestdataStore
{
    public Task<TResponse> GetAsync<TResponse>(string path, int partyId) where TResponse : class, new();
}
    
public class TestdataFileStore(ILogger<TestdataFileStore> logger) : FileStore, ITestdataStore
{
    public async Task<TResponse> GetAsync<TResponse>(string path, int partyId) where TResponse : class, new()
    {
        var response = await GetForParty<TResponse>(path, partyId);
        if (response is not null)
        {
            logger.LogInformation("Returning SPECIFIC testdata for partyId [{partyId}] from [{path}]", partyId, path);
            return response;
        }

        logger.LogInformation("Returning DEFAULT testdata for partyId [{partyId}] from [{path}]", partyId, path);
        return await GetDefault<TResponse>(path);
    }
}