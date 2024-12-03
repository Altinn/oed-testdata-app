using oed_testdata.Server.Infrastructure.TestdataStore;

namespace oed_testdata.Server.Testdata.Estate;

public static class EstateMapper
{
    public static EstateDto Map(EstateData estateData)
    {
        return new EstateDto
        {
            EstateSsn = estateData.EstateSsn,
            EstateName = estateData.EstateName,
            Heirs = estateData.Data.DaCaseList.Single().Parter.Select(p => new Heir
            {
                Ssn = p.Nin,
                Relation = p.Role
            })
        };
    }
}