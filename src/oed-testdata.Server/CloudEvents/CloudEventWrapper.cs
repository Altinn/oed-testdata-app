using CloudNative.CloudEvents.SystemTextJson;
using CloudNative.CloudEvents;
using System.Net.Mime;

namespace oed_testdata.Server.CloudEvents;

/// <summary>
/// Wrapper for overriding deserialization of cloudevents when using minimal API ()
/// Ref: https://gist.github.com/davidfowl/41bcbccc7d8408af57ec1253ca558775
/// </summary>
/// <param name="item"></param>
public struct CloudEventWrapper(CloudEvent item)
{
    public CloudEvent Item { get; } = item;

    public static async ValueTask<CloudEventWrapper> BindAsync(HttpContext context)
    {
        var formatter = new JsonEventFormatter();

        var item = await formatter.DecodeStructuredModeMessageAsync(
            context.Request.BodyReader.AsStream(),
            new ContentType("application/cloudevents+json"), null);

        return new CloudEventWrapper(item);
    }
}