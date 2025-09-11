using oed_testdata.Server.Infrastructure.Maskinporten.Models;

namespace oed_testdata.Server.Testdata.Person;

public static class PersonMapper
{
    public static List<PersonDto> Map(List<TenorDocument> tenorDocuments)
    {
        return tenorDocuments.Select(doc => { return Map(doc); }).ToList();
    }

    public static PersonDto Map(TenorDocument tenorDocument)
    {
        return new PersonDto
        {
            Nin = tenorDocument.Id,
            Name = tenorDocument.DisplayName
        };
    }
}
