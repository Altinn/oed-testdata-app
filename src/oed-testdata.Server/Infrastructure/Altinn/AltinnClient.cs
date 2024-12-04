using Altinn.Platform.Storage.Interface.Models;
using oed_testdata.Server.Infrastructure.Altinn.Models;

namespace oed_testdata.Server.Infrastructure.Altinn;

public interface IAltinnClient
{
    public Task<List<Instance>> GetAllOedInstances();
    public Task<List<Instance>> GetOedInstancesByDeceasedNin(string deceasedNin);
    public Task<List<Instance>> GetOedDeclarationInstancesByDeceasedNin(string deceasedNin);
    public Task<T> GetInstanceData<T>(string partyId, string instanceId, string instanceDataId);
}

public class AltinnClient(HttpClient httpClient) : IAltinnClient
{
    public async Task<List<Instance>> GetAllOedInstances()
    {
        const string path = "/storage/api/v1/instances?org=digdir&appId=digdir/oed&status.isHardDeleted=false&status.isSoftDeleted=false&size=50";

        var response = await httpClient.GetAsync(path);

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        var altinnResponse = await AltinnJsonSerializer.Deserialize<AltinnInstancesResponse>(contentStream);

        return altinnResponse.Instances;
    }

    public async Task<List<Instance>> GetOedInstancesByDeceasedNin(string deceasedNin)
    {
        const string path = "/storage/api/v1/instances?org=digdir&appId=digdir/oed&status.isHardDeleted=false&status.isSoftDeleted=false";

        var request = new HttpRequestMessage(HttpMethod.Get, path);
        request.Headers.TryAddWithoutValidation("X-Ai-InstanceOwnerIdentifier", $"person:{deceasedNin}");
        var response = await httpClient.SendAsync(request);
        
        await using var contentStream = await response.Content.ReadAsStreamAsync();
        var altinnResponse = await AltinnJsonSerializer.Deserialize<AltinnInstancesResponse>(contentStream);

        return altinnResponse.Instances;
    }

    public async Task<List<Instance>> GetOedDeclarationInstancesByDeceasedNin(string deceasedNin)
    {
        const string path = "/storage/api/v1/instances?org=digdir&appId=digdir/oed-declaration&status.isHardDeleted=false";

        var request = new HttpRequestMessage(HttpMethod.Get, path);
        request.Headers.TryAddWithoutValidation("X-Ai-InstanceOwnerIdentifier", $"person:{deceasedNin}");
        var response = await httpClient.SendAsync(request);

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        var altinnResponse = await AltinnJsonSerializer.Deserialize<AltinnInstancesResponse>(contentStream);

        return altinnResponse.Instances;
    }

    public async Task<T> GetInstanceData<T>(string partyId, string instanceId, string instanceDataId)
    {
        var path = $"/storage/api/v1/instances/{partyId}/{instanceId}/data/{instanceDataId}";

        var response = await httpClient.GetAsync(path);

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        var data = AltinnXmlSerializer.Deserialize<T>(contentStream);

        return data;
    }
}