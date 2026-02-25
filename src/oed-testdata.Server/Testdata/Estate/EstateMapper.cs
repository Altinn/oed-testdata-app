using oed_testdata.Server.Infrastructure.TestdataStore.Estate;

namespace oed_testdata.Server.Testdata.Estate;

public static class EstateMapper
{
    public static EstateDto Map(EstateData estateData)
    {
        return new EstateDto
        {
            EstateSsn = estateData.EstateSsn,
            EstateName = estateData.EstateName,
            Metadata = new EstateMetadataDto
            {
                Persons = estateData.Metadata.Persons.Select(p => new EstateMetadataPersonDto
                {
                    Nin = p.Nin,
                    Name = p.Name,
                    OrgNum = p.OrganisasjonsNummer!
                }),
                Tags = [.. estateData.Metadata.Tags]
            },
            Heirs = estateData.Data.DaCaseList.Single().Parter.Select(Map)
        };
    }

    public static HeirDto Map(Part part) => new()
    {
        Ssn = part is PersonPart person ? person.Nin : "",
        OrgNum = part is ForetakPart foretak ? foretak.OrganisasjonsNummer : "",
        Relation = part.Role.ToString(),
        Type = part switch
        {
            PersonPart => "Person",
            ForetakPart => "Foretak",
            _ => "Unknown"
        }
    };

    public static EstateMetadataPerson MapMetadata(Part part) => part switch
    {
        PersonPart person => new EstateMetadataPerson
        {
            Nin = person.Nin,
            Name = person.AdditionalProperties.TryGetValue("name", out var name) ? name?.ToString() : ""
        },
        ForetakPart foretak => new EstateMetadataPerson
        {
            OrganisasjonsNummer = foretak.OrganisasjonsNummer,
            Name = foretak.AdditionalProperties.TryGetValue("name", out var name) ? name?.ToString() : ""
        },
        _ => throw new InvalidOperationException("Failed to parse Part")
    };
}