using System.Text.Json.Serialization;

namespace oed_testdata.Server.Infrastructure.Maskinporten.Models;

public class TenorWrapper
{
    [JsonPropertyName("dokumentListe")]
    public List<TenorDocument> Documents { get; set; } = new List<TenorDocument>();
}

public class TenorDocument
{
    [JsonPropertyName("visningnavn")]
    public string DisplayName { get; set; } = string.Empty;
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}
