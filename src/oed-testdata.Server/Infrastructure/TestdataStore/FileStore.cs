using System.Text.Json;

namespace oed_testdata.Server.Infrastructure.TestdataStore;

public abstract class FileStore
{
    private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    protected async Task<TResponse> GetDefault<TResponse>(string path) where TResponse : class, new()
    {
        EnsureDirectory(path);

        var file = Path.Combine(path, "default.json");
        if (!File.Exists(file))
            return new TResponse();

        await using var fileStream = File.OpenRead(file);
        var data = await JsonSerializer.DeserializeAsync<TResponse>(fileStream, SerializerOptions);

        return data ?? new TResponse();
    }

    protected async Task<TResponse?> GetForParty<TResponse>(string path, int partyId) where TResponse : class, new()
    {
        EnsureDirectory(path);

        var files = Directory.EnumerateFiles(path);
        var file = files.SingleOrDefault(f => f.Contains(partyId.ToString()));

        if (file is null) return null;

        await using var fileStream = File.OpenRead(file);
        var data = await JsonSerializer.DeserializeAsync<TResponse>(fileStream, SerializerOptions);

        return data ?? new TResponse();
    }

    protected static void EnsureDirectory(string path)
    {
        if (!Directory.Exists(path))
            throw new Exception("No path");
    }
}