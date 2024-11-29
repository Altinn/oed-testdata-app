﻿using Altinn.Platform.Storage.Interface.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using oed_testdata.Api.Infrastructure.Altinn;
using oed_testdata.Api.Models;

namespace oed_testdata.Api.Oed
{
    public static class InstanceEndpoints
    {
        public static void MapOedInstanceEndpoints(this WebApplication app)
        {
            app.MapGroup("/api/oed/instance").MapEndpoints();
            //.RequireAuthorization();
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

            var instanceId = instances.First().Id;
            var instanceDataId = instances.First().Data.First(data => data.ContentType == "application/xml").Id;

            var data = await altinnClient.GetOedInstanceData<OED_M>(instanceId, instanceDataId);

            return TypedResults.Ok(data);
        }
    }
}
