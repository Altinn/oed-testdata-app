using Altinn.Platform.Storage.Interface.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using oed_testdata.Server.Infrastructure.Altinn;
using oed_testdata.Server.Models;

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
            group.MapGet("/{estateSsn}/data", GetInstanceDataByEstateSsn);

            return group;
        }

        private static async Task<Ok<List<Instance>>> GetSingleByEstateSsn(IAltinnClient altinnClient, string estateSsn)
        {
            var instances = await altinnClient.GetOedInstancesByDeceasedNin(estateSsn);
            return TypedResults.Ok(instances);
        }

        private static async Task<Ok<OED_M>> GetInstanceDataByEstateSsn(IAltinnClient altinnClient, string estateSsn)
        {
            var instances = await altinnClient.GetOedInstancesByDeceasedNin(estateSsn);

            var partyId = instances.First().InstanceOwner.PartyId;
            var oedInstanceGuid = instances.First().Data.First().InstanceGuid;
            var oedInstanceDataGuid = instances.First().Data.First(data => data.ContentType == "application/xml").Id;

            var data = await altinnClient.GetInstanceData<OED_M>(partyId, oedInstanceGuid, oedInstanceDataGuid);

            return TypedResults.Ok(data);
        }
    }
}
