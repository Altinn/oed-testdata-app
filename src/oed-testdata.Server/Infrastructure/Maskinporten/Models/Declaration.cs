namespace oed_testdata.Server.Infrastructure.Maskinporten.Models
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum MaritalStatusDeclarationEnum
    {

        [System.Runtime.Serialization.EnumMember(Value = @"ugift")]
        Ugift = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"gift")]
        Gift = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"separert")]
        Separert = 2,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public enum UndividedEstateOptionEnum
    {

        [System.Runtime.Serialization.EnumMember(Value = @"vetIkke")]
        VetIkke = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"ja")]
        Ja = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"nei")]
        Nei = 2,

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AgriculturalProperty
    {

        [System.Text.Json.Serialization.JsonPropertyName("municipalityLandNumber")]
        public int? MunicipalityLandNumber { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("propertyUnitNumber")]
        public int? PropertyUnitNumber { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("municipalityNumber")]
        public int MunicipalityNumber { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AgriculturalPropertyClaims
    {

        [System.Text.Json.Serialization.JsonPropertyName("agriculturalProperties")]
        public System.Collections.Generic.ICollection<AgriculturalProperty> AgriculturalProperties { get; set; } = new System.Collections.ObjectModel.Collection<AgriculturalProperty>();

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class AgriculturalPropertyFromRegister
    {

        [System.Text.Json.Serialization.JsonPropertyName("municipalityLandNumber")]
        public int MunicipalityLandNumber { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("propertyUnitNumber")]
        public int PropertyUnitNumber { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("municipalityNumber")]
        public int MunicipalityNumber { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class CohabitantClaim
    {

        [System.Text.Json.Serialization.JsonPropertyName("nin")]
        public string? Nin { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("firstname")]
        public string Firstname { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("lastname")]
        public string Lastname { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("streetAddress")]
        public string? StreetAddress { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("postalCode")]
        public string? PostalCode { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("city")]
        public string? City { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Declaration
    {

        [System.Text.Json.Serialization.JsonPropertyName("maritalStatusClaim")]
        public MaritalStatusClaim MaritalStatusClaim { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("cohabitantClaim")]
        public CohabitantClaim CohabitantClaim { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("undividedEstateClaim")]
        public UndividedEstateInformation UndividedEstateClaim { get; set; } = new UndividedEstateInformation();

        [System.Text.Json.Serialization.JsonPropertyName("testamentClaims")]
        public TestamentClaims TestamentClaims { get; set; } = new TestamentClaims();

        [System.Text.Json.Serialization.JsonPropertyName("marriagePactClaims")]
        public MarriagePactClaims MarriagePactClaims { get; set; } = new MarriagePactClaims();

        [System.Text.Json.Serialization.JsonPropertyName("marriagePactsFromRegister")]
        public System.Collections.Generic.ICollection<MarriagePactFromRegister> MarriagePactsFromRegister { get; set; } = new System.Collections.ObjectModel.Collection<MarriagePactFromRegister>();

        [System.Text.Json.Serialization.JsonPropertyName("agriculturalPropertyClaims")]
        public AgriculturalPropertyClaims AgriculturalPropertyClaims { get; set; } = new AgriculturalPropertyClaims();

        [System.Text.Json.Serialization.JsonPropertyName("agriculturalPropertiesFromRegister")]
        public System.Collections.Generic.ICollection<AgriculturalPropertyFromRegister> AgriculturalPropertiesFromRegister { get; set; } = new System.Collections.ObjectModel.Collection<AgriculturalPropertyFromRegister>();

        [System.Text.Json.Serialization.JsonPropertyName("signatureClaims")]
        public SignatureClaims SignatureClaims { get; set; } = new SignatureClaims();

        [System.Text.Json.Serialization.JsonPropertyName("heirs")]
        public System.Collections.Generic.ICollection<Heir> Heirs { get; set; } = new System.Collections.ObjectModel.Collection<Heir>();

        [System.Text.Json.Serialization.JsonPropertyName("submittedBy")]
        public string SubmittedBy { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("submitted")]
        public System.DateTimeOffset Submitted { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Heir
    {

        [System.Text.Json.Serialization.JsonPropertyName("nin")]
        public string Nin { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class MaritalStatusClaim
    {

        [System.Text.Json.Serialization.JsonPropertyName("status")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public MaritalStatusDeclarationEnum Status { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class MarriagePact
    {

        [System.Text.Json.Serialization.JsonPropertyName("dateOfSignature")]
        public System.DateTimeOffset? DateOfSignature { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class MarriagePactClaims
    {

        [System.Text.Json.Serialization.JsonPropertyName("marriagePacts")]
        public System.Collections.Generic.ICollection<MarriagePact> MarriagePacts { get; set; } = new System.Collections.ObjectModel.Collection<MarriagePact>();

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class MarriagePactFromRegister
    {

        [System.Text.Json.Serialization.JsonPropertyName("dateOfSignature")]
        public System.DateTimeOffset DateOfSignature { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("spouseFullName")]
        public string SpouseFullName { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Signature
    {

        [System.Text.Json.Serialization.JsonPropertyName("heirNin")]
        public string HeirNin { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("acceptsDebt")]
        public bool AcceptsDebt { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("voids60DaysDeadline")]
        public bool Voids60DaysDeadline { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class SignatureClaims
    {

        [System.Text.Json.Serialization.JsonPropertyName("signatures")]
        public System.Collections.Generic.ICollection<Signature> Signatures { get; set; } = new System.Collections.ObjectModel.Collection<Signature>();

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class Testament
    {

        [System.Text.Json.Serialization.JsonPropertyName("dateOfSignature")]
        public System.DateTimeOffset? DateOfSignature { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class TestamentClaims
    {

        [System.Text.Json.Serialization.JsonPropertyName("testaments")]
        public System.Collections.Generic.ICollection<Testament> Testaments { get; set; } = new System.Collections.ObjectModel.Collection<Testament>();

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UndividedEstateClaim
    {

        [System.Text.Json.Serialization.JsonPropertyName("nin")]
        public string? Nin { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("firstname")]
        public string Firstname { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("lastname")]
        public string Lastname { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("dateOfDeath")]
        public System.DateTimeOffset DateOfDeath { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("municipalityNumber")]
        public int MunicipalityNumber { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class UndividedEstateInformation
    {

        [System.Text.Json.Serialization.JsonPropertyName("undividedOption")]
        [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
        public UndividedEstateOptionEnum UndividedOption { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("undividedEstateClaim")]
        public UndividedEstateClaim UndividedEstateClaim { get; set; } = default!;

    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.1.0.0 (NJsonSchema v11.0.2.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial class ProbateDeclarationSubmittedTestModel
    {

        [System.Text.Json.Serialization.JsonPropertyName("avdode")]
        public string Avdode { get; set; } = default!;

        [System.Text.Json.Serialization.JsonPropertyName("caseId")]
        public string CaseId { get; set; } = default!;

    }
}
