using System.Text.Json.Serialization;

namespace oed_testdata.Server.Infrastructure.Maskinporten.Models;

public class Declaration
{
    [JsonPropertyName("signatureClaims")]
    public List<Signature>? SignatureClaims { get; set; }

    [JsonPropertyName("heirs")]
    public List<Heir>? Heirs { get; set; }

    [JsonPropertyName("submittedBy")]
    public string? SubmittedBy { get; set; }

    [JsonPropertyName("submitted")]
    public DateTime? Submitted { get; set; }
}