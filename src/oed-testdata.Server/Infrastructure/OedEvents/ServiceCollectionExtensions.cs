using Microsoft.Extensions.Options;
using oed_testdata.Server.Infrastructure.Altinn;

namespace oed_testdata.Server.Infrastructure.OedEvents;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOedClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<OedSettings>(configuration.GetSection("OedEventsSettings"));

        services
            .AddTransient<AltinnAuthHandler>()
            .AddHttpClient<IOedClient, OedClient>((provider, client) =>
            {
                var settings = provider.GetRequiredService<IOptionsMonitor<OedSettings>>();
                client.BaseAddress = new Uri(settings.CurrentValue.BaseAddress);
            })
            .AddHttpMessageHandler<AltinnAuthHandler>();
            
        return services;
    }
}