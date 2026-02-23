using System.Text.Json;
using System.Text.Json.Serialization;
using CloudNative.CloudEvents;
using oed_testdata.Server.Infrastructure.Altinn;
using oed_testdata.Server.Infrastructure.Auth;
using oed_testdata.Server.Infrastructure.Maskinporten;
using oed_testdata.Server.Infrastructure.OedEvents;
using oed_testdata.Server.Infrastructure.TestdataStore.Estate;

namespace oed_testdata.Server.CloudEvents;

public static class CloudEventEndpoints
{
    public static void MapCloudEventEndpoints(this WebApplication app)
    {
        app
            .MapGroup("/api/cloudevents")
            .MapEndpoints()
            .RequireAuthorization(AuthorizationPolicies.CloudEvents);
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group
            .MapPost("/", ReceiveCloudEvent)
            .Accepts<CloudEvent>("application/cloudevents+json", "application/json");

        return group;
    }

    private static async Task<IResult> ReceiveCloudEvent(
        CloudEventWrapper cloudEventWrapper,
        IEstateStore store,
        IAltinnClient altinnClient,
        IMaskinportenClient maskinportenClient,
        IOedClient oedClient,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(CloudEventEndpoints).FullName!);
        var cloudEvent = cloudEventWrapper.Item;

        logger.LogInformation("Received cloud event type [{CloudEventType}]", cloudEvent.Type);

        // Unknown events are ignored 
        if (cloudEvent.Type != CloudEventType.DeclarationSubmitted)
        {
            logger.LogInformation("Ignoring unknown cloud event type [{CloudEventType}]", cloudEvent.Type);
            return TypedResults.Ok();
        }

        var estateSsn = cloudEvent.Subject;
        var estate = await store.GetByEstateSsn(estateSsn!);

        // If the subject is unknown we ignore the event
        if (estate == null)
        {
            logger.LogInformation("Ignoring cloud event for unknown subject [{Subject}]", cloudEvent.Subject);
            return TypedResults.Ok();
        }

        // OK, we shuld handle this event....
        logger.LogInformation("Handling cloud event for subject [{Subject}]", cloudEvent.Subject);

        var eventData = (cloudEvent.Data as JsonElement?)?.Deserialize<DeclarationSubmittedData>();
        
        var declarationInstances = await altinnClient.GetOedDeclarationInstancesByDeceasedNin(estate.EstateSsn);
        var partyId = declarationInstances.First().InstanceOwner.PartyId;
        var oedDeclarationInstanceGuid = declarationInstances.First().Data.First().InstanceGuid;

        var declaration = await maskinportenClient.GetDeclaration(partyId, oedDeclarationInstanceGuid);
        
        var daCase = estate.Data.DaCaseList.First();
        daCase.SakId = eventData?.DaCaseId ?? daCase.SakId;

        // Do we have alle the data we need to issue the probate? If not, ignore the event
        if (declaration.Heirs is null or {Count: 0} || declaration.SignatureClaims?.Signatures is null or  {Count: 0})
        {
            logger.LogInformation("Ignoring cloud event due to missing data for subject [{Subject}]", cloudEvent.Subject);
            return TypedResults.Ok();
        }

        // Issue probate based on data from the declaration
        daCase.Status = "FERDIGBEHANDLET";
        daCase.ResultatType = "PRIVAT_SKIFTE_IHT_ARVELOVEN_PARAGRAF_99";
        daCase.Skifteattest = new Skifteattest
        {
            Resultat = "PRIVAT_SKIFTE_IHT_ARVELOVEN_PARAGRAF_99",
            Arvinger = daCase.Parter
                .OfType<PersonPart>()
                .Select((p, i) => new SkifteattestArvingPerson
                {
                    Type = "Person",
                    Nin = p.Nin,
                    PaatarGjeldsansvar =
                        declaration.SignatureClaims.Signatures.Any(s => s.AcceptsDebt && s.HeirNin == p.Nin)
                })
                .ToArray(),
        };

        // Setter første arving som påtar seg gjeldsansvar til mottaker av original skifteattest
        daCase.Parter
            .OfType<PersonPart>()
            .Single(p => p.Nin == daCase.Skifteattest.Arvinger.First().Nin)
            .MottakerOriginalSkifteattest = true;
        
        await oedClient.PostDaEvent(estate.Data);
        logger.LogInformation("Issued probate for subject [{Subject}]", cloudEvent.Subject);

        return TypedResults.Ok();
    }
}

public class DeclarationSubmittedData
{
    [JsonPropertyName("daCaseId")]
    public required string DaCaseId { get; set; }
}
