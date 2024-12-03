using System.Text;
using System.Text.Json;
using oed_testdata.Server.Infrastructure.TestdataStore;

namespace oed_testdata.Server.Infrastructure.OedEvents;

public interface IOedEventsClient
{
    public Task PostDaEvent(DaData data);
}

public class OedEventsClient(HttpClient httpClient) : IOedEventsClient
{
    public async Task PostDaEvent(DaData data)
    {
        var json = JsonSerializer.Serialize(data);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("/digdir/oed-events/da-events/api/v1/test", content);
        response.EnsureSuccessStatusCode();
    }
}