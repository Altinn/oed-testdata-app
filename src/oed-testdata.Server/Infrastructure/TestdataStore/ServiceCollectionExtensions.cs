namespace oed_testdata.Server.Infrastructure.TestdataStore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTestdataStore(this IServiceCollection services)
    {
        return services
            .AddTransient<IEstateStore, EstateFileStore>()
            .AddTransient<IBankStore, BankFileStore>();
    }
}