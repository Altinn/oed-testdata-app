using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Altinn.App.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using oed_testdata.Server.Infrastructure.Altinn;
using oed_testdata.Server.Infrastructure.OedEvents;
using oed_testdata.Server.Infrastructure.TestdataStore.Estate;

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

        private static async Task<Ok<IEnumerable<EstateDto>>> GetAll(
            [FromHeader(Name = "X-Feature-IncludeHiddenEstates")]bool? showHidden,
            IEstateStore store)
        {
            var data = await store.ListAll();
            var filtereData = showHidden.HasValue && showHidden.Value
                ? data
                : data.Where(estate => !EstateConstants.HiddenEstates.Contains(estate.EstateSsn));

            return TypedResults.Ok(filtereData.Select(EstateMapper.Map));
        }

        private static async Task<Ok<EstateDto>> GetSingleByEstateSsn(IEstateStore store, string estateSsn)
        {
            var data = await store.GetByEstateSsn(estateSsn);
            return TypedResults.Ok(EstateMapper.Map(data));
        }

        private static async Task<IResult> CreateOrRecreateEstate(
            IEstateStore store, 
            ILoggerFactory loggerFactory, 
            IOedClient oedClient,
            IAltinnClient altinnClient,
            [FromBody]CreateOrUpdateEstateRequest request)
        {
            var logger = loggerFactory.CreateLogger(typeof(EstateEndpoints));
            
            if (string.IsNullOrWhiteSpace(request.EstateSsn))
                return TypedResults.BadRequest();
            
            try
            {
                // Get estate from testapp store (files)
                var estate = await store.GetByEstateSsn(request.EstateSsn);
                if (estate is null)
                {
                    return TypedResults.BadRequest();
                }

                // Delete any existing declarations for this estate
                var declarationInstances = await altinnClient.GetOedDeclarationInstancesByDeceasedNin(estate.EstateSsn);
                if (declarationInstances is { Count: > 0 })
                {
                    var partyId = declarationInstances.First().InstanceOwner.PartyId;
                    var declarationInstanceGuid = declarationInstances.First().Data.First().InstanceGuid;
                    await oedClient.DeleteOedDeclarationInstance(partyId, declarationInstanceGuid);
                }

                // Delete any existing instance data for this estate
                var estateInstances = await altinnClient.GetOedInstancesByDeceasedNin(estate.EstateSsn);
                if (estateInstances is { Count: > 0 })
                {
                    var partyId = estateInstances.First().InstanceOwner.PartyId;
                    var estateInstanceGuid = estateInstances.First().Data.First().InstanceGuid;
                    await oedClient.DeleteOedInstance(partyId, estateInstanceGuid);
                }

                // Update estate data and post DA event to create/recreate estate from scratch
                var data = estate.Data;
                data.SetMottattStatus();
                data.UpdateTimestamps(DateTimeOffset.Now);

                await oedClient.PostDaEvent(data);

                return TypedResults.Ok(EstateMapper.Map(estate));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Ooops!");
                return TypedResults.BadRequest();
            }
        }

        private static async Task<IResult> PatchEstate(
            IEstateStore store,
            ILoggerFactory loggerFactory,
            IOedClient oedClient,
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
                var estate = await store.GetByEstateSsn(request.EstateSsn);
                var data = estate.Data;
                var daCase = data.DaCaseList.Single();

                if (request.Status is not null)
                {
                    daCase.Status = request.Status.ToString()!;
                }

                if (request.ResultatType is not null)
                {
                    daCase.Status = "FERDIGBEHANDLET";
                    daCase.ResultatType = request.ResultatType;

                    // NB! Skifteattest vil KUN bli populert med data fra DA for privat skifte, for alle andre skifteformer vil denne være null
                    if (request.ResultatType.StartsWith("PRIVAT_SKIFTE"))
                    {
                        daCase.Skifteattest = new Skifteattest
                        {
                            Resultat = request.ResultatType,
                            Arvinger = daCase.Parter
                                .Select(p => p.Nin)
                                .ToArray(),
                            ArvingerSomPaatarSegGjeldsansvar = [daCase.Parter.First().Nin]
                        };
                    }
                    else
                    {
                        daCase.Skifteattest = null;
                    }
                }

                data.UpdateTimestamps(DateTimeOffset.Now);
                await oedClient.PostDaEvent(data);
                
                return TypedResults.Ok(EstateMapper.Map(estate));
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

        public string? ResultatType { get; set; }
    }
}
