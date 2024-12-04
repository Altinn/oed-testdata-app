using Microsoft.Extensions.Options;

namespace oed_testdata.Server.Infrastructure.Altinn;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAltinnClient(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<AltinnSettings>(configuration.GetSection("AltinnSettings"));

        services
            .AddMemoryCache()
            .AddTransient<AltinnAuthHandler>()
            .AddHttpClient<IAltinnClient, AltinnClient>((provider, client) =>
            {
                var settings = provider.GetRequiredService<IOptionsMonitor<AltinnSettings>>();
                client.BaseAddress = new Uri(settings.CurrentValue.PlatformUrl);
            })
            .AddHttpMessageHandler<AltinnAuthHandler>();

        return services;
    }
}