using Altinn.ApiClients.Maskinporten.Config;
using Altinn.ApiClients.Maskinporten.Extensions;
using Altinn.ApiClients.Maskinporten.Services;

namespace oed_testdata.Server.Infrastructure.Oed;

public static class ServiceCollectionExtension
{
    private const string ConfigSectionName = "MaskinportenSettings";

    public static IServiceCollection AddMaskinportenClient(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(ConfigSectionName)
            .Get<MaskinportenSettings>();

        settings!.Scope = "digdir:dd:probatedeclarations";

        services.AddMaskinportenHttpClient<SettingsJwkClientDefinition>(MaskinportenConstants.HttpClientName, settings);

        return services;
    }
}

public static class MaskinportenConstants
{
    public const string HttpClientName = "Maskinporten";
}