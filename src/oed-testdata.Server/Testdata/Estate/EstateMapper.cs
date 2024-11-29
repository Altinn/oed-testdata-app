using oed_testdata.Server.Infrastructure.TestdataStore;

namespace oed_testdata.Server.Testdata.Estate;

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