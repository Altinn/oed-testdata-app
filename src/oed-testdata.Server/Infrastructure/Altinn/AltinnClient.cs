using Altinn.Platform.Storage.Interface.Models;
using Microsoft.Extensions.Options;
using oed_testdata.Server.Infrastructure.Altinn.Models;

namespace oed_testdata.Server.Infrastructure.Altinn;

public interface IAltinnClient
{
    public Task<List<Instance>> GetOedInstances();
    public Task<List<Instance>> GetOedInstancesByDeceasedNin(string deceasedNin);
    public Task<List<Instance>> GetOedDeclarationInstancesByDeceasedNin(string deceasedNin);
    public Task<T> GetOedInstanceData<T>(string instanceId, string instanceDataId);
}

public class AltinnClient(
    HttpClient httpClient,
    IOptionsMonitor<AltinnSettings> options) 
    : IAltinnClient
{
    public async Task<List<Instance>> GetOedInstances()
    {
        var baseUri = new Uri(options.CurrentValue.PlatformUrl, UriKind.Absolute);
        var requestUri = new Uri(baseUri, "/storage/api/v1/instances?org=digdir&appId=digdir/oed&status.isHardDeleted=false&status.isSoftDeleted=false&size=50");

        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        var response = await httpClient.SendAsync(request);

        var s = response.Content.ReadAsStringAsync();

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        var altinnResponse = await AltinnJsonSerializer.Deserialize<AltinnInstancesResponse>(contentStream);

        return altinnResponse.Instances;
    }

    public async Task<List<Instance>> GetOedInstancesByDeceasedNin(string deceasedNin)
    {
        var baseUri = new Uri(options.CurrentValue.PlatformUrl, UriKind.Absolute);
        var requestUri = new Uri(baseUri, "/storage/api/v1/instances?org=digdir&appId=digdir/oed&status.isHardDeleted=false&status.isSoftDeleted=false");

        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.TryAddWithoutValidation("X-Ai-InstanceOwnerIdentifier", $"person:{deceasedNin}");
        var response = await httpClient.SendAsync(request);
        
        await using var contentStream = await response.Content.ReadAsStreamAsync();
        var altinnResponse = await AltinnJsonSerializer.Deserialize<AltinnInstancesResponse>(contentStream);

        return altinnResponse.Instances;
    }

    public async Task<List<Instance>> GetOedDeclarationInstancesByDeceasedNin(string deceasedNin)
    {
        var baseUri = new Uri(options.CurrentValue.PlatformUrl, UriKind.Absolute);
        var requestUri = new Uri(baseUri, "/storage/api/v1/instances?org=digdir&appId=digdir/oed-declaration&status.isHardDeleted=false");

        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.TryAddWithoutValidation("X-Ai-InstanceOwnerIdentifier", $"person:{deceasedNin}");
        var response = await httpClient.SendAsync(request);

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        var altinnResponse = await AltinnJsonSerializer.Deserialize<AltinnInstancesResponse>(contentStream);

        return altinnResponse.Instances;
    }

    public async Task<T> GetOedInstanceData<T>(string instanceId, string instanceDataId)
    {
        var baseUri = new Uri(options.CurrentValue.PlatformUrl, UriKind.Absolute);
        var requestUri = new Uri(baseUri, $"https://platform.tt02.altinn.no/storage/api/v1/instances/{instanceId}/data/{instanceDataId}");

        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        var response = await httpClient.SendAsync(request);

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        var data = AltinnXmlSerializer.Deserialize<T>(contentStream);

        return data;
    }
}