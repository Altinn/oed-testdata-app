using System.Text.Json.Serialization;

namespace oed_testdata.Server.Infrastructure.TestdataStore.Estate;

#pragma warning disable CS8618
public class DaData
{
    [JsonPropertyName("DaEventList")]
    public required DaEvent[][] DaEventList { get; set; }

    [JsonPropertyName("DaCaseList")]
    public required DaCase[] DaCaseList { get; set; }


    public void UpdateTilgangsdato(DateTimeOffset? date)
    {
        DaCaseList.Single().TilgangsdatoDigitaltDodsbo = date;
    }

    public void UpdateTimestamps(DateTimeOffset timestamp)
    {
        DaEventList.First().First().Time = timestamp;

        var daCase = DaCaseList.First();
        daCase.ReceivedDate = timestamp;
        daCase.DeadlineDate = timestamp.AddDays(60);
    }

    public void SetFeilfortStatus()
    {
        DaCaseList.Single().Status = "FEILFORT";
    }

    public void SetMottattStatus()
    {
        DaCaseList.Single().Status = "MOTTATT";
    }
}

public class DaEvent
{
    [JsonPropertyName("specversion")]
    public required string Specversion { get; set; }

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("source")]
    public required string Source { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("datacontenttype")]
    public required string DataContentType { get; set; }

    [JsonPropertyName("time")]
    public required DateTimeOffset Time { get; set; }

    [JsonPropertyName("data")]
    public required Data Data { get; set; }
}

public class Data
{
    [JsonPropertyName("@id")]
    public string Id { get; set; }
}

public class DaCase
{
    [JsonPropertyName("sakId")]
    public required string SakId { get; set; }

    [JsonPropertyName("saksnummer")]
    public required string Saksnummer { get; set; }

    [JsonPropertyName("avdode")]
    public required string Avdode { get; set; }

    [JsonPropertyName("embete")]
    public required string Embete { get; set; }

    [JsonPropertyName("status")]
    public required string Status { get; set; }

    [JsonPropertyName("parter")]
    public required Parter[] Parter { get; set; }

    [JsonPropertyName("receivedDate")]
    public required DateTimeOffset ReceivedDate { get; set; }

    [JsonPropertyName("deadlineDate")]
    public required DateTimeOffset DeadlineDate { get; set; }

    [JsonPropertyName("resultatType")]
    public string? ResultatType { get; set; }

    [JsonPropertyName("skifteattest")]
    public Skifteattest? Skifteattest { get; set; }

    [JsonPropertyName("tilgangsdatoDigitaltDodsbo")]
    public DateTimeOffset? TilgangsdatoDigitaltDodsbo { get; set; }

    [JsonPropertyName("erAnnullert")]
    public bool? ErAnnullert { get; set; }
}

public class Skifteattest
{
    [JsonPropertyName("arvinger")]
    public required SkifteattestArvingPerson[] Arvinger { get; set; }

    [JsonPropertyName("resultat")]
    public required string Resultat { get; set; }
}

public class SkifteattestArvingPerson
{
    [JsonPropertyName("type")]
    public required string Type { get; set; } = "Person";
    [JsonPropertyName("nin")]
    public string Nin { get; set; }
    [JsonPropertyName("paatarGjeldsansvar")]
    public bool PaatarGjeldsansvar { get; set; }
}

public class Parter
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "Person";

    [JsonPropertyName("nin")]
    public string Nin { get; set; }

    [JsonPropertyName("organisasjonsNummer")]
    public string? OrganisasjonsNummer { get; set; }

    [JsonPropertyName("role")]
    public required string Role { get; set; }

    [JsonPropertyName("formuesfullmakt")]
    public bool Formuesfullmakt { get; set; }

    [JsonPropertyName("signertDato")]
    public DateTimeOffset? SignertDato { get; set; }

    [JsonPropertyName("onsketSkifteform")]
    public string? OnsketSkifteform { get; set; }

    [JsonPropertyName("paatarGjeldsansvar")]
    public bool PaatarGjeldsansvar { get; set; }

    [JsonPropertyName("godkjennerSkifteattest")]
    public bool GodkjennerSkifteattest { get; set; }

    [JsonPropertyName("mottakerOriginalSkifteattest")]
    public bool MottakerOriginalSkifteattest { get; set; }
}

#pragma warning restore CS8618