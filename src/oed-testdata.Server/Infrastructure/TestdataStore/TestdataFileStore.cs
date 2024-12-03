using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace oed_testdata.Server.Infrastructure.TestdataStore;

public class TestdataFileStore(IMemoryCache cache) : ITestdataStore
{
    private const string EstatePath = "./Testdata/Json/Estate";
    
    public async Task<IEnumerable<DaData>> ListAll()
    {
        var cacheKey = $"{nameof(TestdataFileStore)}.{nameof(ListAll)}";

        var daDataList = await cache.GetOrCreateAsync(
            cacheKey, 
            async (_) => await InternalListAll(),
            new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(86300) });
        
        return daDataList!;
    }

    public async Task<DaData> GetByEstateSsn(string estateSsn)
    {
        var daDataList = await ListAll();
        var estateData = daDataList.SingleOrDefault(item => item.DaCaseList.Single().Avdode == estateSsn);

        if (estateData is null) throw new Exception("Not found");

        return estateData;
    }

    private async Task<IEnumerable<DaData>> InternalListAll()
    {
        EnsureDirectory();

        var daDataList = new List<DaData>();
        foreach (var file in Directory.EnumerateFiles(EstatePath))
        {
            await using var filestream = File.OpenRead(file);
            var daData = await JsonSerializer.DeserializeAsync<DaData>(filestream);

            daData!.UpdateTimestamps(DateTimeOffset.UtcNow);
            daDataList.Add(daData);
        }

        return daDataList;
    }

    private static void EnsureDirectory()
    {
        if (!Directory.Exists(EstatePath))
            throw new Exception("No path");
    }
}