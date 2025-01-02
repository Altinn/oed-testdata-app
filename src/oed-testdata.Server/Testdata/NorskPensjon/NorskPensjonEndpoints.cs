using oed_testdata.Server.Infrastructure.TestdataStore;
using oed_testdata.Server.Infrastructure.TestdataStore.NorskPensjon;

namespace oed_testdata.Server.Testdata.NorskPensjon;

public static class NorskPensjonEndpoints
{
    public static void MapNorskPensjonEndpoints(this WebApplication app)
    {
        app
            .MapGroup("/api/testdata/externalapi/norskpensjon/{instanceOwnerPartyId:int}/{instanceGuid:guid}")
            .MapEndpoints()
            .AllowAnonymous();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetInsurances);
        return group;
    }

    private static async Task<IResult> GetInsurances(
        int instanceOwnerPartyId,
        HttpContext httpContext,
        ITestdataStore store,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(NorskPensjonEndpoints));
        logger.LogInformation("Handling call for {path}", httpContext.Request.Path.Value);

        var resp = await store.GetAsync<PensionResponse>("./Testdata/Json/NorskPensjon", instanceOwnerPartyId);
        return Results.Ok(resp);
    }

}