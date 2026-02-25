using System.Text.Json.Serialization;

namespace oed_testdata.Server.Infrastructure.Maskinporten.Models;

public record struct TenorQueryParameters(
    string? Nin,
    bool? IsDeceased,
    bool? WithRelations,
    int? Count,
    int? MaxAmountOfChildren);

public record struct TenorSearchQuery(
    bool IsDeceased,
    bool IsNorwegianCitizen,
    string Nin,
    bool WithRelations,
    int MaxAmountOfChildren,
    int Count);

public class TenorSearchQueryBuilder
{
    private TenorSearchQuery _query = new();

    public TenorSearchQueryBuilder() { }

    public TenorSearchQueryBuilder WithNorwegianCitizenship(bool isNorwegianCitizen = true)
    {
        _query.IsNorwegianCitizen = isNorwegianCitizen;
        return this;
    }

    public TenorSearchQueryBuilder WithRelations(bool? withRelations)
    {
        _query.WithRelations = withRelations ?? false;
        return this;
    }

    public TenorSearchQueryBuilder WithNin(string? nin)
    {
        _query.Nin = nin ?? string.Empty;
        return this;
    }

    public TenorSearchQueryBuilder WithCount(int? count)
    {
        _query.Count = count ?? 10;
        return this;
    }

    public TenorSearchQueryBuilder WithPersonStatus(bool? isDeceased)
    {
        _query.IsDeceased = isDeceased ?? false;
        return this;
    }

    public TenorSearchQueryBuilder WithAmountOfChildren(int? amountOfChildren)
    {
        _query.MaxAmountOfChildren = amountOfChildren ?? 0;
        return this;
    }

    public string Build()
    {
        var kqlParams = new List<string>();
        var pathParams = new List<string>();
        if (_query.IsNorwegianCitizen)
        {
            kqlParams.Add("norskStatsborgerskap:true");
        }
        else
        {
            kqlParams.Add("norskStatsborgerskap:false");
        }

        if (_query.WithRelations)
        {
            pathParams.Add("vis=tenorRelasjoner.freg.tenorRelasjonsnavn,tenorRelasjoner.freg.visningnavn,tenorRelasjoner.freg.id,id,visningnavn");
        }
        else
        {
            pathParams.Add($"nokkelinformasjon=true");
        }

        if (_query.MaxAmountOfChildren > 0)
        {
            kqlParams.Add($"antallBarn:%5B1+to+{_query.MaxAmountOfChildren}%5D");
        }

        if (_query.Count > 0)
        {
            pathParams.Add($"antall={_query.Count}");
        }

        if (!string.IsNullOrEmpty(_query.Nin))
        {
            kqlParams.Add($"id:+\"{_query.Nin}\"");
        }

        if (_query.IsDeceased)
        {
            kqlParams.Add("personstatus:+\"doed\"");
        }
        else
        {
            kqlParams.Add("personstatus:+\"bosatt\"");
        }

        var path = string.Join('&', pathParams);
        var kql = "?kql=" + string.Join("+and+", kqlParams);
        return kql + "&" + path;
    }
}

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

    [JsonPropertyName("tenorRelasjoner")]
    public FregRelation Relations { get; set; } = new FregRelation();
}

public class FregRelation
{
    [JsonPropertyName("freg")]
    public List<FregRelationItem> Items { get; set; } = new List<FregRelationItem>();
}

public class FregRelationItem
{
    [JsonPropertyName("visningnavn")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("tenorRelasjonsnavn")]
    public string Relation { get; set; } = string.Empty;
}