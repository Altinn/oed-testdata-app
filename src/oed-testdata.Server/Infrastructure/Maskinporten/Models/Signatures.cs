using System.Text.Json.Serialization;

namespace oed_testdata.Server.Infrastructure.Maskinporten.Models;

public class Signature
{
    [JsonPropertyName("heirNin")]
    public required string HeirNin { get; set; }

    [JsonPropertyName("acceptsDebt")]
    public bool AcceptsDebt { get; set; }
}