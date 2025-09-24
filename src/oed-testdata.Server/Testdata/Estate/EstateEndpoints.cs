using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
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
            group.MapPost("/add", AddNewEstate);
            group.MapPatch("/{estateSsn}", PatchEstate);

            return group;
        }

        private static async Task<Ok<IEnumerable<EstateDto>>> GetAll(IEstateStore store)
        {
            var data = await store.ListAll();
            return TypedResults.Ok(data.Select(EstateMapper.Map));
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
            [FromBody] CreateOrUpdateEstateRequest request)
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

        private static async Task<IResult> AddNewEstate(
            IEstateStore store,
            ILoggerFactory loggerFactory,
            [FromBody] AddNewEstate payload)
        {
            var logger = loggerFactory.CreateLogger(typeof(EstateEndpoints));
            if (string.IsNullOrWhiteSpace(payload.EstateSsn))
                return TypedResults.BadRequest();

            var allEstates = await store.ListAll();
            var allDeceasedNins = allEstates.Select(e => e.EstateSsn).ToList();
            if (allDeceasedNins.Contains(payload.EstateSsn))
            {
                logger.LogWarning("Estate with ssn {EstateSsn} already exists", payload.EstateSsn);
                return TypedResults.Conflict();
            }

            var daEventGuid = Guid.NewGuid();
            var daEvent = new DaEvent
            {
                Id = daEventGuid.ToString(),
                Specversion = "1.0",
                Source = "https://domstol.no",
                Type = "DODSFALLSAK-STATUS_OPPDATERT",
                DataContentType = "application/json",
                Time = DateTimeOffset.Now,
                Data = new Data
                {
                    Id = $"https://hendelsesliste.test.domstol.no/api/objects/{daEventGuid}"
                }
            };
            var daData = new DaData
            {
                DaEventList = new DaEvent[][]
                {
                    [
                        new DaEvent
                        {
                            Id = daEventGuid.ToString(),
                            Specversion = "1.0",
                            Source = "https://domstol.no",
                            Type = "DODSFALLSAK-STATUS_OPPDATERT",
                            DataContentType = "application/json",
                            Time = DateTimeOffset.Now,
                            Data = new Data
                            {
                                Id = $"https://hendelsesliste.test.domstol.no/api/objects/{daEventGuid.ToString()}"
                            }
                        }
                    ]
                },
                DaCaseList =
                [
                    new DaCase
                    {
                        DeadlineDate = DateTimeOffset.UtcNow.AddDays(60),
                        ReceivedDate = DateTimeOffset.UtcNow,
                        SakId = daEventGuid.ToString(),
                        Saksnummer = "25-000011DFA-TOSL/07",
                        Avdode = payload.EstateSsn,
                        Embete = "Oslo tingrett",
                        Status = "MOTTATT",
                        Parter = (payload.Heirs ?? []).Select(heir =>
                            new Parter
                            {
                                Formuesfullmakt = true,
                                GodkjennerSkifteattest = false,
                                PaatarGjeldsansvar = false,
                                Role = heir.Relation,
                                Nin = heir.Ssn
                            }).ToArray(),
                    }
                ]
            };
            var estate = new EstateData
            {
                EstateName = payload.DeceasedName,
                EstateSsn = payload.EstateSsn,
                Data = daData,
            };
            await store.Create(estate);
            var createdEstate = await store.GetByEstateSsn(payload.EstateSsn);
            return TypedResults.Ok(EstateMapper.Map(createdEstate));
        }
    }


    public class CreateOrUpdateEstateRequest
    {
        public required string EstateSsn { get; init; }
    }

    public class AddNewEstate
    {
        public required string EstateSsn { get; init; }

        public required string DeceasedName { get; init; }

        public List<Heir>? Heirs { get; init; }

        public List<string>? Tags { get; init; }
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
