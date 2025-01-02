using oed_testdata.Server.Infrastructure.TestdataStore.Kartverket;

namespace oed_testdata.Server.Testdata.Kartverket;

public static class KartverketEndpoints
{
    public static void MapKartverketEndpoints(this WebApplication app)
    {
        app
            .MapGroup("/api/testdata/externalapi/kartverket/{instanceOwnerPartyId:int}/{instanceGuid:guid}")
            .MapEndpoints()
            .AllowAnonymous();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetProperties);
        return group;
    }

    private static async Task<IResult> GetProperties(
        int instanceOwnerPartyId,
        HttpContext httpContext,
        IKartverketStore store,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(KartverketEndpoints));
        logger.LogInformation("Handling call for {path}", httpContext.Request.Path.Value);

        var resp = await store.GetProperties(instanceOwnerPartyId);
        return Results.Ok(resp);
    }
}