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
        group.MapGet("/search", GetPersonByQuery);
        return group;
    }

    public record struct PersonQuery(string? Nin, int? Count, bool? IsDeceased, bool? WithRelations, int? MaxAmountOfChildren);

    private static async Task<Ok<List<PersonDto>>> GetPersonByQuery(
        IMaskinportenClient mpClient,
        IEstateStore estateStore,
        [AsParameters] PersonQuery query)
    {
        var tenorWrapper = await mpClient.TenorSearch(PersonMapper.Map(query));
        var documents = tenorWrapper.Documents;
        
        // Does not return deceased if existing deceased
        var estates = await estateStore.ListAll();
        var deceasedNins = estates.Select(x => x.EstateSsn).ToHashSet();

        documents = tenorWrapper.Documents
            .Where(doc => !deceasedNins.Contains(doc.Id)).ToList();

        return TypedResults.Ok(PersonMapper.Map(documents));
    }
}
