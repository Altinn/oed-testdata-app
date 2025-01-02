using oed_testdata.Server.Infrastructure.TestdataStore;
using oed_testdata.Server.Infrastructure.TestdataStore.Ektepakt;

namespace oed_testdata.Server.Testdata.Ektepakt;

public static class EktepaktEndpoints
{
    public static void MapEktepaktEndpoints(this WebApplication app)
    {
        app
            .MapGroup("/api/testdata/externalapi/ektepakt/{instanceOwnerPartyId:int}/{instanceGuid:guid}")
            .MapEndpoints()
            .AllowAnonymous();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetEktepakter);
        return group;
    }

    private static async Task<IResult> GetEktepakter(
        int instanceOwnerPartyId,
        HttpContext httpContext,
        ITestdataStore store,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(EktepaktEndpoints));
        logger.LogInformation("Handling call for {path}", httpContext.Request.Path.Value);

        var resp = await store.GetAsync<EktepaktResponse>("./Testdata/Json/Ektepakt", instanceOwnerPartyId);
        return Results.Ok(resp);
    }
}