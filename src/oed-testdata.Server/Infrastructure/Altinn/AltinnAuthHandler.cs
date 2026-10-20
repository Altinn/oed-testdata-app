using System.Net.Http.Headers;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace oed_testdata.Server.Infrastructure.Altinn
{
    public class AltinnAuthHandler(
        IHttpClientFactory factory, 
        IOptionsMonitor<AltinnSettings> options,
        IMemoryCache cache)  
        : DelegatingHandler
    {
        private const string Scopes = "altinn:serviceowner/instances.read altinn:serviceowner/instances.write altinn:lookup";

        private static readonly string TokenPathWithQuery = QueryHelpers.AddQueryString(
            "/api/GetEnterpriseToken",
            new Dictionary<string, string?>
            {
                { "env", "tt02" },
                { "orgNo", "991825827" },
                { "org", "digdir" },
                { "ttl", "86400" },
                { "scopes", Scopes }
            });

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization is not null && request.Headers.Authorization.Scheme == "Bearer")
            {
                return await base.SendAsync(request, cancellationToken);
            }

            // Do auth, add auth header and try again
            var token = await GetCachedEnterpriseToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetCachedEnterpriseToken()
        {
            // The token generator request fully determines the token, so its URI is the cache key
            var requestUri = new Uri(new Uri(options.CurrentValue.TokenGeneratorUrl, UriKind.Absolute), TokenPathWithQuery);

            var token = await cache.GetOrCreateAsync(requestUri.ToString(), async (entry) =>
                {
                    return await GetEnterpriseToken(requestUri);
                },
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(86300)
                });

            return token!;
        }

        private async Task<string> GetEnterpriseToken(Uri requestUri)
        {
            var auth = $"{options.CurrentValue.Username}:{options.CurrentValue.Password}";
            var encodedAuth = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(auth));

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", encodedAuth);

            var httpClient = factory.CreateClient(nameof(AltinnAuthHandler));
            var response = await httpClient.SendAsync(request);

            var token = await response.Content.ReadAsStringAsync();

            return token;
        }
    }
}
