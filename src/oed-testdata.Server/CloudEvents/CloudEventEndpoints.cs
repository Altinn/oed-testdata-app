using CloudNative.CloudEvents;
using oed_testdata.Server.Infrastructure.Auth;

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

    private static async Task<IResult> ReceiveCloudEvent(CloudEventWrapper cloudEventWrapper)
    {
        var cloudEvent = cloudEventWrapper.Item;

        await Handle(cloudEvent);
        return TypedResults.Ok();
    }

    private static Task Handle(CloudEvent cloudEvent) => cloudEvent.Type switch
    {
        CloudEventType.WebhookValidation => HandleWebhookValidation(cloudEvent),
        CloudEventType.DeclarationSubmitted => HandleDeclarationSubmitted(cloudEvent),
        _ => Task.CompletedTask
    };

    private static Task HandleWebhookValidation(CloudEvent cloudEvent)
    {
        return Task.CompletedTask;
    }

    private static Task HandleDeclarationSubmitted(CloudEvent cloudEvent)
    {
        return Task.CompletedTask;
    }
}