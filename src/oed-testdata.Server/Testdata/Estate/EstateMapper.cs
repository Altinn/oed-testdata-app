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
                    Name = p.Name
                }),
                Tags = [..estateData.Metadata.Tags]
            },
            Heirs = estateData.Data.DaCaseList.Single().Parter.Select(p => new Heir
            {
                Ssn = p.Nin,
                Name = string.Empty,
                Relation = p.Role
            })
        };
    }
}