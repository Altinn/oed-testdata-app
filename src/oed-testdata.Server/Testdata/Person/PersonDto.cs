using System.Text.Json.Serialization;

namespace oed_testdata.Server.Testdata.Person;

public class PersonDto
{
    public required string Nin { get; init; }
    public required string Name { get; init; }
    public List<RelatedPersonDto>? Relations { get; init; }
}

public class RelatedPersonDto
{
    public required string Name { get; init; }
    public required string Nin { get; init; }
    public required string Relation { get; init; }

}