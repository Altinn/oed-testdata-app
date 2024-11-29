using System.Text.Json;

namespace oed_testdata.Api.Infrastructure.TestdataStore;

public class TestdataFileStore : ITestdataStore
{
    private const string EstatePath = "./Testdata/Json/Estate";

    public async Task<IEnumerable<DaData>> ListAll()
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

    public async Task<DaData> GetByEstateSsn(string estateSsn)
    {
        EnsureDirectory();

        var files = Directory.EnumerateFiles(EstatePath);
        var file = files.SingleOrDefault(f => f.Contains(estateSsn));

        if (file is null) throw new Exception("File not found");

        await using var filestream = File.OpenRead(file);
        var daData = await JsonSerializer.DeserializeAsync<DaData>(filestream);

        daData!.UpdateTimestamps(DateTimeOffset.UtcNow);

        return daData;
    }

    private static void EnsureDirectory()
    {
        if (!Directory.Exists(EstatePath))
            throw new Exception("No path");
    }
}