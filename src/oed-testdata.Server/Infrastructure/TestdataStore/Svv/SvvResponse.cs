using System.Text.Json.Serialization;

namespace oed_testdata.Server.Infrastructure.TestdataStore.Svv;

#pragma warning disable CS8618

public class SvvResponse
{
    [JsonPropertyName("vehicles")]
    public List<Vehicle> Vehicles { get; set; }
}

public class Vehicle
{
    [JsonPropertyName("regNr")] public string RegNr { get; set; }
    [JsonPropertyName("brand")] public string Brand { get; set; }
    [JsonPropertyName("groupName")] public string GroupName { get; set; }
    [JsonPropertyName("groupValue")] public string GroupValue { get; set; }
    [JsonPropertyName("model")] public string Model { get; set; }
    [JsonPropertyName("owner")] public string Owner { get; set; }
    [JsonPropertyName("latestEUApproval")] public DateTime? LatestEUApproval { get; set; }
    [JsonPropertyName("deadlineEUApproval")] public DateTime? DeadlineEUApproval { get; set; }
    [JsonPropertyName("coOwner")] public string CoOwner { get; set; }
    [JsonPropertyName("registrationDate")] public DateTime? RegistrationDate { get; set; }
    [JsonPropertyName("status")] public string Status { get; set; }
}

#pragma warning restore CS8618