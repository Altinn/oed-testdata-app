using System.Text.Json.Serialization;
using Altinn.Platform.Storage.Interface.Models;
using Newtonsoft.Json;

namespace oed_testdata.Api.Infrastructure.Altinn.Models
{
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
}
