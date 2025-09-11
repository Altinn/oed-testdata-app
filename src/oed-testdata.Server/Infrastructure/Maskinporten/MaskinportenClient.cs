using System.Text.Json;
using oed_testdata.Server.Infrastructure.Maskinporten.Models;

namespace oed_testdata.Server.Infrastructure.Maskinporten
{
    public interface IMaskinportenClient
    {
        public Task<Declaration> GetDeclaration(string partyId, string oedDeclarationInstanceGuid);
        public Task<TenorWrapper> TenorSearch(string? nin, bool? deceased = false, int? count = 10);
    }
    
    public class MaskinportenClient(IHttpClientFactory httpClientFactory) : IMaskinportenClient
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient(MaskinportenConstants.HttpClientName);

        public async Task<Declaration> GetDeclaration(string partyId, string oedDeclarationInstanceGuid)
        {
            var path = $"https://digdir.apps.tt02.altinn.no/digdir/oed/api/declarations/{partyId}/{oedDeclarationInstanceGuid}";
            var response = await _httpClient.GetAsync(path);

            response.EnsureSuccessStatusCode();

            await using var contentStream = await response.Content.ReadAsStreamAsync();
            var data = JsonSerializer.Deserialize<Declaration>(contentStream);

            return data!;
        }

        public async Task<TenorWrapper> TenorSearch(string? nin, bool? isDeceased = false, int? count = 10)
        {
            var path = $"https://testdata.api.skatteetaten.no/api/testnorge/v2/soek/freg?kql=";
            var personStatus = "+personstatus:";
            if (isDeceased.HasValue && isDeceased.Value)
            {
                personStatus += "+\"doed\"";
            }
            else
            {
                personStatus += "+\"bosatt\"";
            }

            path += personStatus;
            if (!string.IsNullOrEmpty(nin))
            {
                path += $"+and+id:+\"{nin}\"";
            }

            // Always finish path with this query parameter to get more details
            path += $"&nokkelinformasjon=true&antall={count}";
            var response = await _httpClient.GetAsync(path);

            response.EnsureSuccessStatusCode();

            await using var contentStream = await response.Content.ReadAsStreamAsync();
            var data = JsonSerializer.Deserialize<TenorWrapper>(contentStream);

            return data!;
        }
    }
}
