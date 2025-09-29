using System.Text.Json.Serialization;

namespace oed_testdata.JsonGenerator.Models.Da;
public class DaData
{
    [JsonPropertyName("DaEventList")]
    public required DaEvent[][] DaEventList { get; set; }

    [JsonPropertyName("DaCaseList")]
    public required DaCase[] DaCaseList { get; set; }


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
}

public class Skifteattest
{
    [JsonPropertyName("arvinger")]
    public required string[] Arvinger { get; set; }

    [JsonPropertyName("arvingerSomPaatarSegGjeldsansvar")]
    public required string[] ArvingerSomPaatarSegGjeldsansvar { get; set; }

    [JsonPropertyName("resultat")]
    public required string Resultat { get; set; }
}

public class Parter
{
    [JsonPropertyName("nin")]
    public required string Nin { get; set; }

    [JsonPropertyName("role")]
    [JsonConverter(typeof(JsonStringEnumConverter<PartRole>))]
    public required PartRole Role { get; set; }

    [JsonPropertyName("formuesfullmakt")]
    public required bool Formuesfullmakt { get; set; }

    [JsonPropertyName("signertDato")]
    public DateTimeOffset? SignertDato { get; set; }

    [JsonPropertyName("onsketSkifteform")]
    public string? OnsketSkifteform { get; set; }

    [JsonPropertyName("paatarGjeldsansvar")]
    public required bool PaatarGjeldsansvar { get; set; }

    [JsonPropertyName("godkjennerSkifteattest")]
    public required bool GodkjennerSkifteattest { get; set; }
}


[System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
public enum PartRole
{

    [System.Runtime.Serialization.EnumMember(Value = @"PART_ANNEN")]
    PART_ANNEN = 0,

    [System.Runtime.Serialization.EnumMember(Value = @"GJENLEV_EKTEFELLE_PARTNER")]
    GJENLEV_EKTEFELLE_PARTNER = 1,

    [System.Runtime.Serialization.EnumMember(Value = @"GJENLEV_PARTNER")]
    GJENLEV_PARTNER = 2,

    [System.Runtime.Serialization.EnumMember(Value = @"GJENLEV_SAMBOER")]
    GJENLEV_SAMBOER = 3,

    [System.Runtime.Serialization.EnumMember(Value = @"BARN")]
    BARN = 4,

    [System.Runtime.Serialization.EnumMember(Value = @"BARNEBARN")]
    BARNEBARN = 5,

    [System.Runtime.Serialization.EnumMember(Value = @"SAERKULLSBARN")]
    SAERKULLSBARN = 6,

    [System.Runtime.Serialization.EnumMember(Value = @"SAERKULLSBARN_BARN")]
    SAERKULLSBARN_BARN = 7,

    [System.Runtime.Serialization.EnumMember(Value = @"FAR")]
    FAR = 8,

    [System.Runtime.Serialization.EnumMember(Value = @"MOR")]
    MOR = 9,

    [System.Runtime.Serialization.EnumMember(Value = @"SOESKEN")]
    SOESKEN = 10,

    [System.Runtime.Serialization.EnumMember(Value = @"SOESKENS_BARN")]
    SOESKENS_BARN = 11,

    [System.Runtime.Serialization.EnumMember(Value = @"SOESKENS_BARNEBARN")]
    SOESKENS_BARNEBARN = 12,

    [System.Runtime.Serialization.EnumMember(Value = @"HALV_SOESKEN")]
    HALV_SOESKEN = 13,

    [System.Runtime.Serialization.EnumMember(Value = @"HALV_SOESKENS_BARN")]
    HALV_SOESKENS_BARN = 14,

    [System.Runtime.Serialization.EnumMember(Value = @"FARFAR")]
    FARFAR = 15,

    [System.Runtime.Serialization.EnumMember(Value = @"FARMOR")]
    FARMOR = 16,

    [System.Runtime.Serialization.EnumMember(Value = @"MORFAR")]
    MORFAR = 17,

    [System.Runtime.Serialization.EnumMember(Value = @"MORMOR")]
    MORMOR = 18,

    [System.Runtime.Serialization.EnumMember(Value = @"ONKEL")]
    ONKEL = 19,

    [System.Runtime.Serialization.EnumMember(Value = @"TANTE")]
    TANTE = 20,

    [System.Runtime.Serialization.EnumMember(Value = @"FETTER")]
    FETTER = 21,

    [System.Runtime.Serialization.EnumMember(Value = @"KUSINE")]
    KUSINE = 22,

    [System.Runtime.Serialization.EnumMember(Value = @"STATEN")]
    STATEN = 23,

    [System.Runtime.Serialization.EnumMember(Value = @"AVDOEDE")]
    AVDOEDE = 24,

    [System.Runtime.Serialization.EnumMember(Value = @"FORDRINGSHAVER")]
    FORDRINGSHAVER = 25,

    [System.Runtime.Serialization.EnumMember(Value = @"AVDOEDE_EKTEFELLE_PARTNER")]
    AVDOEDE_EKTEFELLE_PARTNER = 26,

    [System.Runtime.Serialization.EnumMember(Value = @"TEST_ARVING_FULL")]
    TEST_ARVING_FULL = 27,

    [System.Runtime.Serialization.EnumMember(Value = @"TEST_ARVING_BEGR")]
    TEST_ARVING_BEGR = 28,

    [System.Runtime.Serialization.EnumMember(Value = @"VERGE")]
    VERGE = 29,

    [System.Runtime.Serialization.EnumMember(Value = @"FORELDREVERGE_AKTOER")]
    FORELDREVERGE_AKTOER = 30,

    [System.Runtime.Serialization.EnumMember(Value = @"MIDLERTIDIGVERGE_AKTOER")]
    MIDLERTIDIGVERGE_AKTOER = 31,

    [System.Runtime.Serialization.EnumMember(Value = @"FULLMEKTIG")]
    FULLMEKTIG = 32,

    [System.Runtime.Serialization.EnumMember(Value = @"PROSFULL")]
    PROSFULL = 33,

    [System.Runtime.Serialization.EnumMember(Value = @"FREMTIDSFULLMEKTIG")]
    FREMTIDSFULLMEKTIG = 34,

    [System.Runtime.Serialization.EnumMember(Value = @"GJENLEV_EKTEFELLE")]
    GJENLEV_EKTEFELLE = 35,

    [System.Runtime.Serialization.EnumMember(Value = @"MOTTAKER_FULLMAKT_DOEDSBO_LITEN_VERDI")]
    MOTTAKER_FULLMAKT_DOEDSBO_LITEN_VERDI = 36,

    [System.Runtime.Serialization.EnumMember(Value = @"BARNEBARNS_BARN")]
    BARNEBARNS_BARN = 37,

    [System.Runtime.Serialization.EnumMember(Value = @"SOESKENS_BARNEBARNS_BARN")]
    SOESKENS_BARNEBARNS_BARN = 38,

    [System.Runtime.Serialization.EnumMember(Value = @"HALV_SOESKENS_BARNEBARN")]
    HALV_SOESKENS_BARNEBARN = 39,

}