using oed_testdata.Server.Infrastructure.TestdataStore.Bank;
using oed_testdata.Server.Infrastructure.TestdataStore.Ektepakt;
using oed_testdata.Server.Infrastructure.TestdataStore.Estate;
using oed_testdata.Server.Infrastructure.TestdataStore.Kartverket;
using oed_testdata.Server.Infrastructure.TestdataStore.Svv;

namespace oed_testdata.Server.Infrastructure.TestdataStore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTestdataStore(this IServiceCollection services)
    {
        return services
            .AddTransient<IEstateStore, EstateFileStore>()
            .AddTransient<IBankStore, BankFileStore>()
            .AddTransient<ISvvStore, SvvFileStore>()
            .AddTransient<IKartverketStore, KartverketFileStore>()
            .AddTransient<IEktepaktStore, EktepaktFileStore>();
    }
}