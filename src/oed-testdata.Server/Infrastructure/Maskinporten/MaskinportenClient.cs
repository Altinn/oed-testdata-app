using System.Text.Json;
using System.Web;
using oed_testdata.Server.Infrastructure.Maskinporten.Models;

namespace oed_testdata.Server.Infrastructure.Maskinporten
{
    public interface IMaskinportenClient
    {
        public Task<Declaration> GetDeclaration(string partyId, string oedDeclarationInstanceGuid);
        public Task<TenorWrapper> TenorSearch(string? nin, bool? deceased = false, int? count = 10, bool? withRelations = false);
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

        public async Task<TenorWrapper> TenorSearch(string? nin, bool? isDeceased = false, int? count = 10, bool? fullRelations = false)
        {
            var path = $"https://testdata.api.skatteetaten.no/api/testnorge/v2/soek/freg?kql=";
            var personStatus = "+norskStatsborgerskap:true+and+personstatus:";
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

            if (fullRelations.HasValue && fullRelations.Value)
            {
                // antallBarn:[1+to+3]
                path += "+and+antallBarn:%5B1+to+3%5D&vis=tenorRelasjoner.freg.tenorRelasjonsnavn,tenorRelasjoner.freg.visningnavn,tenorRelasjoner.freg.id,id,visningnavn";
            }
            else
            {
                path += $"&nokkelinformasjon=true";
            }
            // Always finish path with this query parameter to get more details
            path += $"&antall={count}";
            var response = await _httpClient.GetAsync(path);

            response.EnsureSuccessStatusCode();

            await using var contentStream = await response.Content.ReadAsStreamAsync();
            var data = JsonSerializer.Deserialize<TenorWrapper>(contentStream);

            return data!;
        }
    }
}
