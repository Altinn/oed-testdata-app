namespace oed_testdata.Api.Infrastructure.Altinn;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMaskinportenClient(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<AltinnSettings>(configuration.GetSection("AltinnSettings"));

        services
            .AddMemoryCache()
            .AddTransient<MaskinportenAuthHandler>()
            .AddHttpClient<IAltinnClient, AltinnClient>()
            .AddHttpMessageHandler<MaskinportenAuthHandler>();

        return services;
    }
}