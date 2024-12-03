using Microsoft.AspNetCore.Authentication.Cookies;

namespace oed_testdata.Server.Infrastructure.Auth
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBasicAuthentication(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.Configure<AuthSettings>(configuration.GetSection("AuthSettings"));

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
    }
}
