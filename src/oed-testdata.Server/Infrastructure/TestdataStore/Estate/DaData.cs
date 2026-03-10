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
    public required Part[] Parter { get; set; }

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
    public ICollection<ArvingSkifteattest> Arvinger { get; set; }

    [JsonPropertyName("resultat")]
    public required string Resultat { get; set; }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(PersonSkifteattest), typeDiscriminator: "Person")]
[JsonDerivedType(typeof(PersonPappSkifteattest), typeDiscriminator: "PersonPapp")]
[JsonDerivedType(typeof(ForetakSkifteattest), typeDiscriminator: "Foretak")]
[JsonDerivedType(typeof(ForetakPappSkifteattest), typeDiscriminator: "ForetakPapp")]
[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.6.3.0 (NJsonSchema v11.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
public partial class ArvingSkifteattest
{

    [System.Text.Json.Serialization.JsonPropertyName("paatarGjeldsansvar")]
    public bool PaatarGjeldsansvar { get; set; }

    private System.Collections.Generic.IDictionary<string, object> _additionalProperties;

    [System.Text.Json.Serialization.JsonExtensionData]
    public System.Collections.Generic.IDictionary<string, object> AdditionalProperties
    {
        get { return _additionalProperties ?? (_additionalProperties = new System.Collections.Generic.Dictionary<string, object>()); }
        set { _additionalProperties = value; }
    }

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.6.3.0 (NJsonSchema v11.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
public partial class PersonSkifteattest : ArvingSkifteattest
{

    [System.Text.Json.Serialization.JsonPropertyName("nin")]
    [System.ComponentModel.DataAnnotations.RegularExpression(@"^\d{11}$")]
    public string Nin { get; set; }

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.6.3.0 (NJsonSchema v11.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
public partial class PersonPappSkifteattest : ArvingSkifteattest
{

    [System.Text.Json.Serialization.JsonPropertyName("navn")]
    public PersonName Navn { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("dateOfBirth")]
    [System.Text.Json.Serialization.JsonConverter(typeof(DateFormatConverter))]
    public System.DateTimeOffset DateOfBirth { get; set; }

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.6.3.0 (NJsonSchema v11.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
public partial class ForetakSkifteattest : ArvingSkifteattest
{

    [System.Text.Json.Serialization.JsonPropertyName("organisasjonsNummer")]
    public double OrganisasjonsNummer { get; set; }

}

[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.6.3.0 (NJsonSchema v11.5.2.0 (Newtonsoft.Json v13.0.0.0))")]
public partial class ForetakPappSkifteattest : ArvingSkifteattest
{

    [System.Text.Json.Serialization.JsonPropertyName("organisasjonsNavn")]
    public string OrganisasjonsNavn { get; set; }

}


//------------------------------------------------
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
    public double? OrganisasjonsNummer { get; set; }

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