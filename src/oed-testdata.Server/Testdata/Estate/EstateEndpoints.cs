using Microsoft.AspNetCore.Http.HttpResults;
using oed_testdata.Server.Infrastructure.TestdataStore;

namespace oed_testdata.Server.Testdata.Estate
{
    public static class EstateEndpoints
    {
        public static void MapEstateEndpoints(this WebApplication app)
        {
            app.MapGroup("/api/testdata/estate").MapEndpoints();
            //.RequireAuthorization();
        }

        private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll);
            group.MapGet("/{estateSsn}", GetSingleByEstateSsn); 

            return group;
        }

        private static async Task<Ok<IEnumerable<EstateDto>>> GetAll(ITestdataStore store)
        {
            var data = await store.ListAll();
            return TypedResults.Ok(data.Select(EstateMapper.Map));
        }

        private static async Task<Ok<EstateDto>> GetSingleByEstateSsn(ITestdataStore store, string estateSsn)
        {
            var data = await store.GetByEstateSsn(estateSsn);
            return TypedResults.Ok(EstateMapper.Map(data));
        }
    }
}
