using Altinn.Platform.Storage.Interface.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using oed_testdata.Server.Infrastructure.Altinn;

namespace oed_testdata.Server.Oed
{
    public static class InstanceEndpoints
    {
        public static void MapOedInstanceEndpoints(this WebApplication app)
        {
            app
                .MapGroup("/api/oed/instance")
                .MapEndpoints()
                .RequireAuthorization();
        }

        private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
        {
            group.MapGet("/{estateSsn}", GetSingleByEstateSsn);

            return group;
        }

        private static async Task<Ok<List<Instance>>> GetSingleByEstateSsn(IAltinnClient altinnClient, string estateSsn)
        {
            var instances = await altinnClient.GetOedInstancesByDeceasedNin(estateSsn);
            return TypedResults.Ok(instances);
        }
    }
}
