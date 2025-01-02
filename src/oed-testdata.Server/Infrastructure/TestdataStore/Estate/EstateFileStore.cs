using System.Text.Json;

namespace oed_testdata.Server.Infrastructure.TestdataStore.Estate;

public class EstateData
{
    public required string EstateSsn { get; init; }
    public required string EstateName { get; init; }
    public required DaData Data { get; init; }
}

public class EstateFileStore : IEstateStore
{
    private const string EstatePath = "./Testdata/Json/Estate";

    public async Task<IEnumerable<EstateData>> ListAll()
    {
        EnsureDirectory();

        var estateList = new List<EstateData>();
        foreach (var file in Directory.EnumerateFiles(EstatePath))
        {
            await using var filestream = File.OpenRead(file);

            var daData = await JsonSerializer.DeserializeAsync<DaData>(filestream);
            daData!.UpdateTimestamps(DateTimeOffset.UtcNow);

            var estateData = new EstateData
            {
                EstateSsn = daData.DaCaseList.Single().Avdode,
                EstateName = ParseEstateNameFromFileName(file),
                Data = daData
            };

            estateList.Add(estateData);
        }

        return estateList;
    }

    private static string ParseEstateNameFromFileName(string filepath)
    {
        var filename = Path.GetFileNameWithoutExtension(filepath);
        var parts = filename.Split("-");

        if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[1]))
            return "";

        var estateName = parts[1].Replace("_", " ");
        return estateName;
    }

    public async Task<EstateData?> GetByEstateSsn(string estateSsn)
    {
        EnsureDirectory();

        var files = Directory.EnumerateFiles(EstatePath);
        var file = files.SingleOrDefault(f => f.Contains(estateSsn));

        if (file is null) return null;

        await using var filestream = File.OpenRead(file);
        var daData = await JsonSerializer.DeserializeAsync<DaData>(filestream);
        daData!.UpdateTimestamps(DateTimeOffset.UtcNow);

        return new EstateData
        {
            EstateSsn = daData.DaCaseList.Single().Avdode,
            EstateName = ParseEstateNameFromFileName(file),
            Data = daData
        };
    }

    private static void EnsureDirectory()
    {
        if (!Directory.Exists(EstatePath))
            throw new Exception("No path");
    }
}