using System.Text.Json;
using oed_testdata.Server.Infrastructure.Maskinporten.Models;

namespace oed_testdata.Server.Infrastructure.Maskinporten
{
    public interface IMaskinportenClient
    {
        public Task<Declaration> GetDeclaration(string partyId, string oedDeclarationInstanceGuid);
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
    }
}
