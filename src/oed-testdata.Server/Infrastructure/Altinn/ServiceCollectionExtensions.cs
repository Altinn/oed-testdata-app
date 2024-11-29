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
            .AddHttpClient<IAltinnClient, AltinnClient>()
            .AddHttpMessageHandler<AltinnAuthHandler>();

        return services;
    }
}