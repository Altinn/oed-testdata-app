using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace oed_testdata.Server.Infrastructure.Auth
{
    public static class AuthEndpoints
    {
        public static void MapBasicAuthenticationEndpoints(this WebApplication app)
        {
            app.MapGroup("/api/auth").MapEndpoints();
        }

        private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
        {
            group.MapGet("/login", Login);
            return group;
        }
        
        private static IResult Login(
            [FromHeader(Name = "Authorization")] string authHeader, 
            [FromServices] IOptionsMonitor<AuthSettings> options)
        {
            try
            {
                var encodedEmailPassword = authHeader.Substring("Basic ".Length).Trim();
                var emailPassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedEmailPassword));

                var seperatorIndex = emailPassword.IndexOf(':');
                var username = emailPassword.Substring(0, seperatorIndex);
                var password = emailPassword.Substring(seperatorIndex + 1);
                
                if (username != options.CurrentValue.Username || password != options.CurrentValue.Password)
                    return TypedResults.Unauthorized();
                
                var claims = new[]
                {
                    new Claim("Name", "Testuser")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                return TypedResults.SignIn(new ClaimsPrincipal(claimsIdentity),
                    authenticationScheme: CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception)
            {
                return TypedResults.InternalServerError();
            }
        }
    }
}
