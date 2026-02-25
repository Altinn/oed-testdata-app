using oed_testdata.Server.Infrastructure.Maskinporten.Models;
using static oed_testdata.Server.Testdata.Person.PersonEndpoints;

namespace oed_testdata.Server.Testdata.Person;

public static class PersonMapper
{
    public static List<PersonDto> Map(List<TenorDocument> tenorDocuments)
    {
        return tenorDocuments.Select(Map).ToList();
    }

    public static TenorQueryParameters Map(PersonQuery tenorDocuments)
    {
        return new TenorQueryParameters
        {
            Count = tenorDocuments.Count,
            WithRelations = tenorDocuments.WithRelations,
            IsDeceased = tenorDocuments.IsDeceased,
            Nin = tenorDocuments.Nin,
            MaxAmountOfChildren = tenorDocuments.MaxAmountOfChildren
        };
    }

    public static PersonDto Map(TenorDocument tenorDocument)
    {
        return new PersonDto
        {
            Nin = tenorDocument.Id,
            Name = tenorDocument.DisplayName,
            Type = "Person",
            Relations = tenorDocument.Relations.Items.Select(r => new RelatedPersonDto
            {
                Name = r.DisplayName,
                Type = "Person",
                Nin = r.Id,
                Relation = r.Relation
            }).ToList()
        };
    }
}
