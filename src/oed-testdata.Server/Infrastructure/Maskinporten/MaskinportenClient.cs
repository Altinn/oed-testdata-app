using System.Text.Json;
using System.Web;
using oed_testdata.Server.Infrastructure.Maskinporten.Models;

namespace oed_testdata.Server.Infrastructure.Maskinporten
{
    public interface IMaskinportenClient
    {
        public Task<Declaration> GetDeclaration(string partyId, string oedDeclarationInstanceGuid);
        public Task<TenorWrapper> TenorSearch(TenorQueryParameters searchQuery);
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

        public async Task<TenorWrapper> TenorSearch(TenorQueryParameters searchQuery)
        {
            var query = new TenorSearchQueryBuilder()
                .WithNorwegianCitizenship()
                .WithRelations(searchQuery.WithRelations)
                .WithPersonStatus(searchQuery.IsDeceased)
                .WithCount(searchQuery.Count)
                .WithNin(searchQuery.Nin)
                .Build();

            var path = $"https://testdata.api.skatteetaten.no/api/testnorge/v2/soek/freg" + query;
            var response = await _httpClient.GetAsync(path);

            response.EnsureSuccessStatusCode();

            await using var contentStream = await response.Content.ReadAsStreamAsync();
            var data = JsonSerializer.Deserialize<TenorWrapper>(contentStream);

            return data!;
        }
    }
}
