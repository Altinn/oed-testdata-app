using System.Text.Json.Serialization;
using Altinn.Platform.Storage.Interface.Models;
using Newtonsoft.Json;

namespace oed_testdata.Server.Infrastructure.Altinn.Models;

#pragma warning disable CS8618

public class AltinnInstancesResponse
{
    [JsonProperty("count")]
    public int Count { get; set; }

    [JsonPropertyName("self")]
    public string Self { get; set; }

    [JsonPropertyName("next")]
    public string Next { get; set; }

    [JsonPropertyName("instances")]
    public List<Instance> Instances { get; set; }
}

#pragma warning restore CS8618