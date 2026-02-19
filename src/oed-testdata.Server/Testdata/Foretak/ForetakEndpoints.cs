using Microsoft.AspNetCore.Http.HttpResults;
using oed_testdata.Server.Infrastructure.Maskinporten;
using oed_testdata.Server.Infrastructure.TestdataStore.Estate;

namespace oed_testdata.Server.Testdata.Foretak;

public static class ForetakEndpoints
{
    public static void MapForetakEndpoints(this WebApplication app)
    {
        app
            .MapGroup("/api/testdata/companies")
            .MapEndpoints()
            .RequireAuthorization();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/search", GetForetakByQuery);
        return group;
    }

    public record struct ForetakQuery(string? OrgNum, int? Count, string? Type = "AS");

    private static async Task<Ok<List<ForetakDto>>> GetForetakByQuery(
        IMaskinportenClient mpClient,
        IEstateStore estateStore,
        [AsParameters] ForetakQuery query)
    {
        var tenorWrapper = await mpClient.TenorCompanySearch(ForetakMapper.Map(query));

        return TypedResults.Ok(ForetakMapper.Map(tenorWrapper.Documents));
    }
}
