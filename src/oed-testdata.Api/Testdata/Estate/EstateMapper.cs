using oed_testdata.Api.Infrastructure.TestdataStore;

namespace oed_testdata.Api.Testdata.Estate;

public static class EstateMapper
{
    public static EstateDto Map(DaData daData)
    {
        return daData.DaCaseList
            .Select(da => new EstateDto
            {
                EstateSsn = da.Avdode,
                Heirs = da.Parter.Select(p => new Heir
                {
                    Ssn = p.Nin,
                    Relation = p.Role
                })
            })
            .Single();
    }
}