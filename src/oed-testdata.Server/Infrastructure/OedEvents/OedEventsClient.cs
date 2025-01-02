using System.Text;
using System.Text.Json;
using oed_testdata.Server.Infrastructure.TestdataStore.Estate;

namespace oed_testdata.Server.Infrastructure.OedEvents;

public interface IOedClient
{
    public Task PostDaEvent(DaData data);
    public Task DeleteOedInstance(string partyId, string oedInstanceGuid);
    public Task DeleteOedDeclarationInstance(string partyId, string oedDeclarationInstanceGuid);
}

public class OedClient(HttpClient httpClient) : IOedClient
{
    public async Task PostDaEvent(DaData data)
    {
        var json = JsonSerializer.Serialize(data);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        const string path = "/digdir/oed-events/da-events/api/v1/test";

        var response = await httpClient.PostAsync(path, content);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteOedInstance(string partyId, string oedInstanceGuid)
    {
        var path = $"/digdir/oed/instances/{partyId}/{oedInstanceGuid}?hard=true";

        var response = await httpClient.DeleteAsync(path);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteOedDeclarationInstance(string partyId, string oedDeclarationInstanceGuid)
    {
        var path = $"/digdir/oed-declaration/instances/{partyId}/{oedDeclarationInstanceGuid}?hard=true";

        var response = await httpClient.DeleteAsync(path);
        response.EnsureSuccessStatusCode();
    }
}