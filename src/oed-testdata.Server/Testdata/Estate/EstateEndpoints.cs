using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using oed_testdata.Server.Infrastructure.OedEvents;
using oed_testdata.Server.Infrastructure.TestdataStore;

namespace oed_testdata.Server.Testdata.Estate
{
    public static class EstateEndpoints
    {
        public static void MapEstateEndpoints(this WebApplication app)
        {
            app
                .MapGroup("/api/testdata/estate")
                .MapEndpoints()
                .RequireAuthorization();
        }

        private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll);
            group.MapGet("/{estateSsn}", GetSingleByEstateSsn);
            group.MapPost("/", CreateOrRecreateEstate);
            group.MapPatch("/{estateSsn}", PatchEstate);

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

        private static async Task<IResult> CreateOrRecreateEstate(
            ITestdataStore store, 
            ILoggerFactory loggerFactory, 
            IOedEventsClient oedEventsClient,
            [FromBody]CreateOrUpdateEstateRequest request)
        {
            var logger = loggerFactory.CreateLogger(typeof(EstateEndpoints));
            
            if (string.IsNullOrWhiteSpace(request.EstateSsn))
                return TypedResults.BadRequest();
            
            try
            {
                var data = await store.GetByEstateSsn(request.EstateSsn);
                data.SetMottattStatus();
                data.UpdateTimestamps(DateTimeOffset.Now);

                await oedEventsClient.PostDaEvent(data);

                return TypedResults.Ok(EstateMapper.Map(data));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Ooops!");
                return TypedResults.BadRequest();
            }
        }

        private static async Task<IResult> PatchEstate(
            ITestdataStore store,
            ILoggerFactory loggerFactory,
            IOedEventsClient oedEventsClient,
            [FromRoute] string estateSsn,
            [FromBody] PatchEstateRequest request)
        {
            var logger = loggerFactory.CreateLogger(typeof(EstateEndpoints));

            if (string.IsNullOrWhiteSpace(estateSsn) || 
                string.IsNullOrWhiteSpace(request.EstateSsn) ||
                estateSsn != request.EstateSsn)
            {
                return TypedResults.BadRequest();
            }

            try
            {
                var data = await store.GetByEstateSsn(request.EstateSsn);

                if (request.Status is not null)
                {
                    data.DaCaseList.Single().Status = request.Status.ToString()!;
                }

                data.UpdateTimestamps(DateTimeOffset.Now);
                await oedEventsClient.PostDaEvent(data);
                
                return TypedResults.Ok(EstateMapper.Map(data));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Ooops!");
                return TypedResults.BadRequest();
            }
        }

    }


    public class CreateOrUpdateEstateRequest
    {
        public required string EstateSsn { get; init; }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum EstateStatus
    {
        MOTTATT = 0,
        FERDIGBEHANDLET = 1,
        FEILFORT = 2,
    }

    public class PatchEstateRequest
    {
        public required string EstateSsn { get; init; }

        [JsonConverter(typeof(JsonStringEnumConverter<EstateStatus>))]
        public EstateStatus? Status { get; init; }
    }
}
