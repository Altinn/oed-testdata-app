using Microsoft.AspNetCore.Http.HttpResults;
using oed_testdata.Server.Infrastructure.Maskinporten;
using oed_testdata.Server.Infrastructure.TestdataStore.Estate;

namespace oed_testdata.Server.Testdata.Person;

public static class PersonEndpoints
{
    public static void MapPersonEndpoints(this WebApplication app)
    {
        app
            .MapGroup("/api/testdata/people")
            .MapEndpoints()
            .RequireAuthorization();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/search", GetPersonByQuery); // Example with query parameters  
        return group;
    }

    private static async Task<Ok<List<PersonDto>>> GetPersonByQuery(
        IMaskinportenClient altinnClient,
        IEstateStore estateStore,
        string? nin,
        int? count,
        bool? isDeceased,
        bool? excludeExisting)
    {
        var tenorWrapper = await altinnClient.TenorSearch(nin, isDeceased, count);
        var documents = tenorWrapper.Documents;
        if (excludeExisting is true)
        {
            var estates = await estateStore.ListAll();
            var deceasedNins = estates.Select(x => x.EstateSsn).ToHashSet();

            documents = tenorWrapper.Documents
                .Where(doc => !deceasedNins.Contains(doc.Id)).ToList();
        }

        return TypedResults.Ok(PersonMapper.Map(documents));
    }
}
