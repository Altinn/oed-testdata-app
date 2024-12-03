using Microsoft.Extensions.Options;
using oed_testdata.Server.Infrastructure.Altinn;

namespace oed_testdata.Server.Infrastructure.OedEvents
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOedEventsClient(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<OedEventsSettings>(configuration.GetSection("OedEventsSettings"));

            services
                .AddTransient<AltinnAuthHandler>()
                .AddHttpClient<IOedEventsClient, OedEventsClient>((provider, client) =>
                {
                    var settings = provider.GetRequiredService<IOptionsMonitor<OedEventsSettings>>();

                    client.BaseAddress = new Uri(settings.CurrentValue.BaseAddress);
                })
                .AddHttpMessageHandler<AltinnAuthHandler>();


            return services;
        }
    }
}
