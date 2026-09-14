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
        var logger = loggerFactory.CreateLogger(typeof(CloudEventEndpoints));

        if (Environment.GetEnvironmentVariable("AUTO_APPROVE_SUBMITTED_DECLARATIONS") != "true")
        {
            logger.LogWarning("Environment variable [AUTO_APPROVE_SUBMITTED_DECLARATIONS] is not set to 'true'. Cloud events will be ignored.");
            return TypedResults.Ok();
        }

        var cloudEvent = cloudEventWrapper.Item;

        logger.LogInformation("Received cloud event type [{CloudEventType}]", cloudEvent.Type);

        // Unknown events are ignored 
        if (cloudEvent.Type is
            not CloudEventType.DeclarationSubmitted and
            not CloudEventType.DeclarationV2Submitted)
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

        // An individual (v2) declaration is filled in by one heir and carries no
        // SignatureClaims, so the joint flow below cannot read it. Issue from the submitting
        // heir's own answers instead: the event fires once per heir, and the first one
        // completes the case.
        if (cloudEvent.Type == CloudEventType.DeclarationV2Submitted)
        {
            return await IssueProbateFromIndividualDeclaration();
        }

        var declarationInstance = await GetDeclarationInstance();

        // A missing declaration instance is not exceptional. DeclarationV2Submitted events in
        // particular look up dd-private-probate instances by the deceased NIN, but those instances
        // are owned by the submitting heir, so the lookup finds nothing. Ack the event instead of
        // throwing - an unhandled exception here returns 500 and Altinn retries the delivery for
        // half an hour.
        if (declarationInstance is null)
        {
            logger.LogWarning(
                "Ignoring cloud event of type [{CloudEventType}] for subject [{Subject}]: no declaration instance found",
                cloudEvent.Type, cloudEvent.Subject);
            return TypedResults.Ok();
        }

        var partyId = declarationInstance.InstanceOwner.PartyId;

        var declarationDataElement = declarationInstance.Data.FirstOrDefault();
        if (declarationDataElement is null)
        {
            logger.LogWarning(
                "Ignoring cloud event for subject [{Subject}]: declaration instance [{InstanceId}] has no data elements",
                cloudEvent.Subject, declarationInstance.Id);
            return TypedResults.Ok();
        }

        var oedDeclarationInstanceGuid = declarationDataElement.InstanceGuid;

        var declaration = await maskinportenClient.GetDeclaration(partyId, oedDeclarationInstanceGuid);

        var daCase = estate.Data.DaCaseList.First();
        daCase.SakId = eventData?.DaCaseId ?? daCase.SakId;

        // Do we have alle the data we need to issue the probate? If not, ignore the event
        if (declaration.Heirs is null or { Count: 0 } || declaration.SignatureClaims?.Signatures is null or { Count: 0 })
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
                .Select(part =>
                {
                    var arvingSkifteattest = ArvingExtensions.GetArvingSkifteattestFromPart(part);

                    // Setting paatarGjeldsansvar according to the signatures
                    if (arvingSkifteattest is PersonSkifteattest personSkifteattest)
                    {
                        arvingSkifteattest.PaatarGjeldsansvar = declaration.SignatureClaims.Signatures
                            .Any(signature =>
                                signature.AcceptsDebt &&
                                signature.HeirNin == personSkifteattest.Nin);
                    }

                    return arvingSkifteattest;
                })
                .ToArray(),
        };

        // Setter første arving som påtar seg gjeldsansvar til mottaker av original skifteattest
        daCase.Parter
            .OfType<PersonPart>()
            .Single(p => p.Nin == daCase.Skifteattest.Arvinger
                .OfType<PersonSkifteattest>()
                .First(arving => arving.PaatarGjeldsansvar).Nin)
            .MottakerOriginalSkifteattest = true;

        await oedClient.PostDaEvent(estate.Data);
        logger.LogInformation("Issued probate for subject [{Subject}]", cloudEvent.Subject);

        return TypedResults.Ok();

        async Task<IResult> IssueProbateFromIndividualDeclaration()
        {
            if (cloudEvent.Source is null)
            {
                logger.LogWarning(
                    "Ignoring cloud event for subject [{Subject}]: no source to read the declaration from",
                    cloudEvent.Subject);
                return TypedResults.Ok();
            }

            // OED sets the event source to its sub-app declaration endpoint, so the event
            // already names the heir instance and no lookup is needed.
            var individualDeclaration = await maskinportenClient.GetSubAppDeclaration(cloudEvent.Source);

            var individualDaCase = estate.Data.DaCaseList.First();
            individualDaCase.SakId = eventData?.DaCaseId ?? individualDaCase.SakId;

            individualDaCase.Status = "FERDIGBEHANDLET";
            individualDaCase.ResultatType = "PRIVAT_SKIFTE_IHT_ARVELOVEN_PARAGRAF_99";
            individualDaCase.Skifteattest = new Skifteattest
            {
                Resultat = "PRIVAT_SKIFTE_IHT_ARVELOVEN_PARAGRAF_99",
                Arvinger = individualDaCase.Parter
                    .Select(part =>
                    {
                        var arving = ArvingExtensions.GetArvingSkifteattestFromPart(part);

                        // Only the submitting heir has actually answered the debt question.
                        // Everyone else keeps the default their part carries.
                        if (arving is PersonSkifteattest personArving &&
                            personArving.Nin == individualDeclaration.SubmittedBy)
                        {
                            personArving.PaatarGjeldsansvar = individualDeclaration.AcceptsDebt;
                        }

                        return arving;
                    })
                    .ToArray(),
            };

            // Whoever takes on the debt receives the original. Single() would throw when nobody
            // does, which is a legitimate state for an individual declaration.
            var ninsAcceptingDebt = individualDaCase.Skifteattest.Arvinger
                .OfType<PersonSkifteattest>()
                .Where(arving => arving.PaatarGjeldsansvar)
                .Select(arving => arving.Nin)
                .ToHashSet();

            var recipient = individualDaCase.Parter
                .OfType<PersonPart>()
                .FirstOrDefault(part => ninsAcceptingDebt.Contains(part.Nin));

            if (recipient is null)
            {
                logger.LogWarning(
                    "No heir accepts debt for subject [{Subject}]; issuing probate without a MottakerOriginalSkifteattest",
                    cloudEvent.Subject);
            }
            else
            {
                recipient.MottakerOriginalSkifteattest = true;
            }

            await oedClient.PostDaEvent(estate.Data);
            logger.LogInformation(
                "Issued probate for subject [{Subject}] from individual declaration submitted by [{SubmittedBy}]",
                cloudEvent.Subject, individualDeclaration.SubmittedBy);

            return TypedResults.Ok();
        }

        async Task<Altinn.Platform.Storage.Interface.Models.Instance?> GetDeclarationInstance()
        {
            var instances = cloudEvent.Type switch
            {
                CloudEventType.DeclarationSubmitted => await altinnClient.GetOedDeclarationInstancesByDeceasedNin(estate.EstateSsn),
                CloudEventType.DeclarationV2Submitted => await altinnClient.GetDdPrivateProbateInstancesByDeceasedNin(estate.EstateSsn),
                _ => throw new InvalidOperationException($"Unknown cloud event type [{cloudEvent.Type}]"),
            };
        
            return instances.FirstOrDefault();
        }
    }
}

public class DeclarationSubmittedData
{
    [JsonPropertyName("daCaseId")]
    public required string DaCaseId { get; set; }
}
