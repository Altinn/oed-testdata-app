using System.Text.Json.Serialization;

namespace oed_testdata.Server.Infrastructure.Maskinporten.Models;

/// <summary>
/// The part of dd-private-probate's probate declaration this app needs.
///
/// Unlike the joint oed-declaration model, an individual declaration carries no
/// SignatureClaims: it is filled in by one heir, so AcceptsDebt and SubmittedBy describe
/// that heir alone. Heirs is deliberately not read here - the sub-app concatenates its
/// heirsWithPoa and heirsWithoutPoa prefills, so it cannot tell us who holds a power of
/// attorney.
/// </summary>
public class SubAppDeclaration
{
    [JsonPropertyName("acceptsDebt")]
    public bool AcceptsDebt { get; set; }

    [JsonPropertyName("submittedBy")]
    public string? SubmittedBy { get; set; }
}
