using oed_testdata.Server.Infrastructure.Maskinporten.Models;
using static oed_testdata.Server.Testdata.Foretak.ForetakEndpoints;

namespace oed_testdata.Server.Testdata.Foretak;

public static class ForetakMapper
{
    public static List<ForetakDto> Map(List<TenorCompanyDocument> tenorDocuments) => 
        tenorDocuments.Select(Map).ToList();

    public static TenorCompanyQueryParameters Map(ForetakQuery tenorDocuments)
    {
        return new TenorCompanyQueryParameters
        {
            Count = tenorDocuments.Count,
            Type = tenorDocuments.Type,
            OrgNum = tenorDocuments.OrgNum,
        };
    }

    public static ForetakDto Map(TenorCompanyDocument tenorDocument)
    {
        return new ForetakDto
        {
            OrgNum = tenorDocument.Id,
            Name = tenorDocument.DisplayName,
        };
    }
}
