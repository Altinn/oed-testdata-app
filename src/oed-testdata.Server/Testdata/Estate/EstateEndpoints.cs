using System.Security.Claims;
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
            group.MapPost("/", CreateOrUpdateEstate);

            return group;
        }

        private static async Task<Ok<IEnumerable<EstateDto>>> GetAll(ITestdataStore store, ClaimsPrincipal user)
        {
            var data = await store.ListAll();
            return TypedResults.Ok(data.Select(EstateMapper.Map));
        }

        private static async Task<Ok<EstateDto>> GetSingleByEstateSsn(ITestdataStore store, string estateSsn)
        {
            var data = await store.GetByEstateSsn(estateSsn);
            return TypedResults.Ok(EstateMapper.Map(data));
        }
        private static async Task<IResult> CreateOrUpdateEstate(
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

                // Poster først en update med status FEILFORT for å fjerne alle tilganger til boet
                data.UpdateTimestamps(DateTimeOffset.Now - TimeSpan.FromSeconds(1));
                data.SetFeilfortStatus();
                await oedEventsClient.PostDaEvent(data);

                // Poster deretter en vanlig oppdatering av boet som vil oppdatere instansen og populere korrekte roller for alle parter i boet
                data.UpdateTimestamps(DateTimeOffset.Now);
                data.SetMottattStatus();
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


    public record CreateOrUpdateEstateRequest(string EstateSsn);
}
