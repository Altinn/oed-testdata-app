using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace oed_testdata.Server.Infrastructure.Auth
{
    public static class ServiceCollectionExtensions
    {
        private const string ConfigSectionName = "AuthSettings";

        public static IServiceCollection AddBasicAuthentication(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.Configure<AuthSettings>(configuration.GetSection(ConfigSectionName));
            
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Name = "oed-testdata-user";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                    options.Cookie.HttpOnly = true;

                    options.Cookie.SameSite = environment.IsDevelopment()
                        ? SameSiteMode.None
                        : SameSiteMode.Strict;
                });

            return services;
        }

        public static IServiceCollection AddBasicAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            var auth = configuration.GetSection(ConfigSectionName).Get<AuthSettings>();

            services.AddHttpContextAccessor();
            services.AddScoped<IAuthorizationHandler, QueryParamRequirementHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicies.CloudEvents, builder =>
                {
                    builder.AddRequirements(new QueryParamRequirement(
                        auth!.CloudEventQueryParamName,
                        auth.CloudEventSecret));
                });
            });

            return services;
        }
    }

    public static class AuthorizationPolicies
    {
        public const string CloudEvents = "CloudEvents";
    }
}
