using System.Text.Json.Serialization;

namespace oed_testdata.Server.Infrastructure.Maskinporten.Models;

public class Heir
{
    [JsonPropertyName("nin")]
    public required string Nin { get; set; }
}