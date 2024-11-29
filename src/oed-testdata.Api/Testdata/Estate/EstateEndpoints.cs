using Microsoft.AspNetCore.Http.HttpResults;
using oed_testdata.Api.Infrastructure.TestdataStore;

namespace oed_testdata.Api.Testdata.Estate
{
    public static class EstateEndpoints
    {
        public static void MapEstateEndpoints(this WebApplication app)
        {
            app.MapGroup("/api/testdata/estate")
                .MapTestdatApi()
                .WithName("Estate endpoints");

            //.RequireAuthorization();
        }

        public static RouteGroupBuilder MapTestdatApi(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll)
                .WithName(nameof(GetAll));

            group.MapGet("/{estateSsn}", GetSingleByEstateSsn)
                .WithName(nameof(GetSingleByEstateSsn)); 

            return group;
        }

        public static async Task<Ok<IEnumerable<EstateDto>>> GetAll(ITestdataStore store)
        {
            var data = await store.ListAll();
            return TypedResults.Ok(data.Select(EstateMapper.Map));
        }

        public static async Task<Ok<EstateDto>> GetSingleByEstateSsn(ITestdataStore store, string estateSsn)
        {
            var data = await store.GetByEstateSsn(estateSsn);
            return TypedResults.Ok(EstateMapper.Map(data));
        }
    }
}
