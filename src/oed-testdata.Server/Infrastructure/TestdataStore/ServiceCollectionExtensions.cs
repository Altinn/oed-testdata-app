namespace oed_testdata.Server.Infrastructure.TestdataStore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTestdataStore(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddTransient<ITestdataStore, TestdataFileStore>();

        return services;
    }
}