namespace oed_testdata.Server.Testdata.Estate
{
    public class EstateDto
    {
        public required string EstateSsn { get; init; }
        public required IEnumerable<Heir> Heirs { get; init; }
    }

    public class Heir
    {
        public required string Ssn { get; init; }
        public required string Relation { get; init; }
    }
}
