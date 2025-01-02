using oed_testdata.Server.Infrastructure.TestdataStore;
using oed_testdata.Server.Infrastructure.TestdataStore.Svv;

namespace oed_testdata.Server.Testdata.Svv;

public static class SvvEndpoints
{
    public static void MapSvvEndpoints(this WebApplication app)
    {
        app
            .MapGroup("/api/testdata/externalapi/svv/{instanceOwnerPartyId:int}/{instanceGuid:guid}")
            .MapEndpoints()
            .AllowAnonymous();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetVehicles);
        return group;
    }

    private static async Task<IResult> GetVehicles(
        int instanceOwnerPartyId,
        HttpContext httpContext,
        ITestdataStore store,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(SvvEndpoints));
        logger.LogInformation("Handling call for {path}", httpContext.Request.Path.Value);

        var resp = await store.GetAsync<SvvResponse>("./Testdata/Json/Svv", instanceOwnerPartyId);
        return Results.Ok(resp);
    }
}