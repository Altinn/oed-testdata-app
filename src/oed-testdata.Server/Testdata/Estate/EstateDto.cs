namespace oed_testdata.Server.Testdata.Estate
{
    public class EstateDto
    {
        public required string EstateSsn { get; init; }
        public required string EstateName { get; init; }
        public EstateMetadataDto Metadata { get; init; } = new();
        public required IEnumerable<Heir> Heirs { get; init; }
    }

    public class EstateMetadataDto
    {
        public IEnumerable<EstateMetadataPersonDto> Persons { get; init; } = [];
        public IEnumerable<string> Tags { get; init; } = [];
    }

    public class EstateMetadataPersonDto
    {
        public string Nin { get; set; }
        public string Name { get; set; }
    }

    public class Heir
    {
        public required string Ssn { get; init; }
        public required string Relation { get; init; }
    }
}
