using System.Text.Json.Serialization;

namespace oed_testdata.Server.Infrastructure.Maskinporten.Models;

public record struct TenorCompanyQueryParameters(
    string? OrgNum,
    string? Type,
    int? Count);

public record struct TenorCompanySearchQuery(
    string OrgNum,
    string Type,
    int Count);

public class TenorCompanySearchQueryBuilder
{
    private TenorCompanySearchQuery _query = new();

    public TenorCompanySearchQueryBuilder() { }

    public TenorCompanySearchQueryBuilder WithCount(int? count)
    {
        _query.Count = count ?? 10;
        return this;
    }

    public TenorCompanySearchQueryBuilder WithOrgNum(string? orgNum)
    {
        _query.OrgNum = orgNum ?? string.Empty;
        return this;
    }

    public TenorCompanySearchQueryBuilder WithType(string? type)
    {
        _query.Type= type ?? string.Empty;
        return this;
    }

    public string Build()
    {
        var kqlParams = new List<string>();
        var pathParams = new List<string>();
        if (!string.IsNullOrEmpty(_query.Type))
        {
            kqlParams.Add($"organisasjonsform.kode:{_query.Type}");
        }

        pathParams.Add($"nokkelinformasjon=true");
        pathParams.Add("vis=id,visningnavn,navn,tenorMetadata.id");

        if (_query.Count > 0)
        {
            pathParams.Add($"antall={_query.Count}");
        }

        if (!string.IsNullOrEmpty(_query.OrgNum))
        {
            kqlParams.Add($"organisasjonsnummer:{_query.OrgNum}");
        }

        var path = string.Join('&', pathParams);
        var kql = "?kql=" + string.Join("+and+", kqlParams);
        return kql + "&" + path;
    }
}

public class TenorCompanyWrapper
{
    [JsonPropertyName("dokumentListe")]
    public List<TenorCompanyDocument> Documents { get; set; } = [];
}

public class TenorCompanyDocument
{
    [JsonPropertyName("navn")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("organisasjonsnummer")]
    public string Id { get; set; } = string.Empty;
}
