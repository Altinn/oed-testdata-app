using System.Text.Json;

namespace oed_testdata.Server.Infrastructure.TestdataStore.Estate;

public class EstateMetadataPerson()
{
    public required string Nin { get; init; }
    public required string Name { get; init; }
}

public class EstateMetadata()
{
    public List<EstateMetadataPerson> Persons { get; init; } = [];
    public List<string> Tags { get; init; } = [];
}

public class EstateData
{
    public required string EstateSsn { get; init; }
    public required string EstateName { get; init; }
    public EstateMetadata Metadata { get; set; } = new();
    public required DaData Data { get; init; }
}

public class EstateFileStore : IEstateStore
{
    private const string EstatePath = "./Testdata/Json/Estate";
    private const string MetdataPostfix = "-metadata.json";

    private JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

    public async Task<IEnumerable<EstateData>> ListAll()
    {
        EnsureDirectory();

        var estateList = new List<EstateData>();
        var metadataDict = new Dictionary<string, EstateMetadata>();

        foreach (var file in Directory.EnumerateFiles(EstatePath))
        {
            await using var filestream = File.OpenRead(file);

            if (file.EndsWith(MetdataPostfix))
            {
                var metadata = await JsonSerializer.DeserializeAsync<EstateMetadata>(filestream, _serializerOptions);
                var nin = Path.GetFileName(file)[..11];
                metadataDict.Add(nin, metadata!);
            }
            else
            {
                var daData = await JsonSerializer.DeserializeAsync<DaData>(filestream, _serializerOptions);
                daData!.UpdateTimestamps(DateTimeOffset.UtcNow);

                var estateData = new EstateData
                {
                    EstateSsn = daData.DaCaseList.Single().Avdode,
                    EstateName = ParseEstateNameFromFileName(file),
                    Data = daData
                };

                estateList.Add(estateData);
            }
        }

        foreach (var estate in estateList)
        {
            if (metadataDict.TryGetValue(estate.EstateSsn, out var metadata))
            {
                estate.Metadata = metadata;
            }
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
        var file = files.SingleOrDefault(f => !f.EndsWith(MetdataPostfix) && f.Contains(estateSsn));

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

    public async Task Create(EstateData estate)
    {
        var filename = $"{estate.EstateSsn}-{string.Join("_", estate.EstateName.Split(" "))}.json";
        var filepath = Path.Combine("Testdata/Json/Estate", filename);

        await using var filestream = File.Open(filepath, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(filestream, estate.Data, JsonSerializerOptions.Default);
        await filestream.FlushAsync();
    }

    public async Task CreateMetadata(EstateData estate)
    {
        var filename = $"{estate.EstateSsn}-metadata.json";
        var filepath = Path.Combine("Testdata/Json/Estate", filename);

        await using var filestream = File.Open(filepath, FileMode.Create, FileAccess.Write);
        await JsonSerializer.SerializeAsync(filestream, estate.Metadata, JsonSerializerOptions.Web);
        await filestream.FlushAsync();
    }

    private static void EnsureDirectory()
    {
        if (!Directory.Exists(EstatePath))
            throw new Exception("No path");
    }
}