namespace oed_testdata.Server.Testdata.Estate;

#pragma warning disable CS8618
public class EstateDto
{
    public required string EstateSsn { get; init; }
    public required string EstateName { get; init; }
    public EstateMetadataDto Metadata { get; init; } = new();
    public required IEnumerable<HeirDto> Heirs { get; init; }
}

public class EstateMetadataDto
{
    // This can contain other types as well, but leaving the name as "Persons" for backcomp
    public IEnumerable<EstateMetadataPersonDto> Persons { get; init; } = [];
    public IEnumerable<string> Tags { get; init; } = [];
}

public class EstateMetadataPersonDto
{
    public string? Nin { get; set; }
    public string? Name { get; set; }
    public string? OrgNum { get; init; }
}

public class HeirDto
{
    public required string Type { get; init; }
    public string? Ssn { get; init; }
    public string? Relation { get; init; }
    public string? OrgNum { get; init; }
}
#pragma warning restore CS8618