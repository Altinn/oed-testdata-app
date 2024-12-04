#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
namespace Altinn.App.Models
{
  [XmlRoot(ElementName="declaration")]
  public class declaration
  {
    [XmlElement("TestamentClaims", Order = 1)]
    [JsonProperty("TestamentClaims")]
    [JsonPropertyName("TestamentClaims")]
    public List<TestamentClaims> TestamentClaims { get; set; }

    [XmlElement("SF_TestamentClaims", Order = 2)]
    [JsonProperty("SF_TestamentClaims")]
    [JsonPropertyName("SF_TestamentClaims")]
    public List<SF_TestamentClaims> SF_TestamentClaims { get; set; }

    [XmlElement("MarriagePactClaims", Order = 3)]
    [JsonProperty("MarriagePactClaims")]
    [JsonPropertyName("MarriagePactClaims")]
    public List<MarriagePactClaims> MarriagePactClaims { get; set; }

    [XmlElement("SF_MarriagePactClaims", Order = 4)]
    [JsonProperty("SF_MarriagePactClaims")]
    [JsonPropertyName("SF_MarriagePactClaims")]
    public List<SF_MarriagePactClaims> SF_MarriagePactClaims { get; set; }

    [XmlElement("AgriculturalPropertyClaims", Order = 5)]
    [JsonProperty("AgriculturalPropertyClaims")]
    [JsonPropertyName("AgriculturalPropertyClaims")]
    public List<AgriculturalPropertyClaims> AgriculturalPropertyClaims { get; set; }

    [XmlElement("SF_AgriculturalPropertyClaims", Order = 6)]
    [JsonProperty("SF_AgriculturalPropertyClaims")]
    [JsonPropertyName("SF_AgriculturalPropertyClaims")]
    public List<SF_AgriculturalPropertyClaims> SF_AgriculturalPropertyClaims { get; set; }

    [XmlElement("MaritalStatusClaims", Order = 7)]
    [JsonProperty("MaritalStatusClaims")]
    [JsonPropertyName("MaritalStatusClaims")]
    public MaritalStatusClaims MaritalStatusClaims { get; set; }

    [XmlElement("SF_MaritalStatusClaims", Order = 8)]
    [JsonProperty("SF_MaritalStatusClaims")]
    [JsonPropertyName("SF_MaritalStatusClaims")]
    public SF_MaritalStatusClaims SF_MaritalStatusClaims { get; set; }

    [XmlElement("HasAdditionalTestaments", Order = 9)]
    [JsonProperty("HasAdditionalTestaments")]
    [JsonPropertyName("HasAdditionalTestaments")]
    public bool? HasAdditionalTestaments { get; set; }

    public bool ShouldSerializeHasAdditionalTestaments() => HasAdditionalTestaments.HasValue;

    [XmlElement("IsMaritalStatusCorrect", Order = 10)]
    [JsonProperty("IsMaritalStatusCorrect")]
    [JsonPropertyName("IsMaritalStatusCorrect")]
    public bool? IsMaritalStatusCorrect { get; set; }

    public bool ShouldSerializeIsMaritalStatusCorrect() => IsMaritalStatusCorrect.HasValue;

    [XmlElement("SignatureClaims", Order = 11)]
    [JsonProperty("SignatureClaims")]
    [JsonPropertyName("SignatureClaims")]
    public List<SignatureClaims> SignatureClaims { get; set; }

    [XmlElement("ReferrerOedInstanceId", Order = 12)]
    [JsonProperty("ReferrerOedInstanceId")]
    [JsonPropertyName("ReferrerOedInstanceId")]
    public string ReferrerOedInstanceId { get; set; }

    [XmlElement("ProbatePrefill", Order = 13)]
    [JsonProperty("ProbatePrefill")]
    [JsonPropertyName("ProbatePrefill")]
    public ProbatePrefill ProbatePrefill { get; set; }

    [XmlElement("HasAdditionalMarriagePacts", Order = 14)]
    [JsonProperty("HasAdditionalMarriagePacts")]
    [JsonPropertyName("HasAdditionalMarriagePacts")]
    public bool? HasAdditionalMarriagePacts { get; set; }

    public bool ShouldSerializeHasAdditionalMarriagePacts() => HasAdditionalMarriagePacts.HasValue;

    [XmlElement("CurrentUserHasAcceptedTerms", Order = 15)]
    [JsonProperty("CurrentUserHasAcceptedTerms")]
    [JsonPropertyName("CurrentUserHasAcceptedTerms")]
    public bool? CurrentUserHasAcceptedTerms { get; set; }

    public bool ShouldSerializeCurrentUserHasAcceptedTerms() => CurrentUserHasAcceptedTerms.HasValue;

    [XmlElement("CurrentUserHasAcceptedEstateLowValue", Order = 16)]
    [JsonProperty("CurrentUserHasAcceptedEstateLowValue")]
    [JsonPropertyName("CurrentUserHasAcceptedEstateLowValue")]
    public string CurrentUserHasAcceptedEstateLowValue { get; set; }

    [XmlElement("HasAdditionalAgriProperties", Order = 17)]
    [JsonProperty("HasAdditionalAgriProperties")]
    [JsonPropertyName("HasAdditionalAgriProperties")]
    public bool? HasAdditionalAgriProperties { get; set; }

    public bool ShouldSerializeHasAdditionalAgriProperties() => HasAdditionalAgriProperties.HasValue;

    [XmlElement("CurrentUserHasAcceptedPublicDivision", Order = 18)]
    [JsonProperty("CurrentUserHasAcceptedPublicDivision")]
    [JsonPropertyName("CurrentUserHasAcceptedPublicDivision")]
    public string CurrentUserHasAcceptedPublicDivision { get; set; }

    [XmlElement("CurrentUserHasAcceptedUndividedEstate", Order = 19)]
    [JsonProperty("CurrentUserHasAcceptedUndividedEstate")]
    [JsonPropertyName("CurrentUserHasAcceptedUndividedEstate")]
    public string CurrentUserHasAcceptedUndividedEstate { get; set; }

    [XmlElement("CurrentUserHasAcceptedDebt", Order = 20)]
    [JsonProperty("CurrentUserHasAcceptedDebt")]
    [JsonPropertyName("CurrentUserHasAcceptedDebt")]
    public bool? CurrentUserHasAcceptedDebt { get; set; }

    public bool ShouldSerializeCurrentUserHasAcceptedDebt() => CurrentUserHasAcceptedDebt.HasValue;

    [XmlElement("CurrentUsersChosenProbateType", Order = 21)]
    [JsonProperty("CurrentUsersChosenProbateType")]
    [JsonPropertyName("CurrentUsersChosenProbateType")]
    public string CurrentUsersChosenProbateType { get; set; }

    [XmlElement("SF_MarriagePacts", Order = 22)]
    [JsonProperty("SF_MarriagePacts")]
    [JsonPropertyName("SF_MarriagePacts")]
    public string SF_MarriagePacts { get; set; }

    [XmlElement("SF_Agriculture", Order = 23)]
    [JsonProperty("SF_Agriculture")]
    [JsonPropertyName("SF_Agriculture")]
    public string SF_Agriculture { get; set; }

    [XmlElement("SF_Heirs", Order = 24)]
    [JsonProperty("SF_Heirs")]
    [JsonPropertyName("SF_Heirs")]
    public string SF_Heirs { get; set; }

    [XmlElement("SF_PartyName", Order = 25)]
    [JsonProperty("SF_PartyName")]
    [JsonPropertyName("SF_PartyName")]
    public string SF_PartyName { get; set; }

    [XmlElement("SF_Testament", Order = 26)]
    [JsonProperty("SF_Testament")]
    [JsonPropertyName("SF_Testament")]
    public string SF_Testament { get; set; }

    [XmlElement("SF_MaritalStatus", Order = 27)]
    [JsonProperty("SF_MaritalStatus")]
    [JsonPropertyName("SF_MaritalStatus")]
    public string SF_MaritalStatus { get; set; }

    [XmlElement("IsTestamentsCorrect", Order = 28)]
    [JsonProperty("IsTestamentsCorrect")]
    [JsonPropertyName("IsTestamentsCorrect")]
    public bool? IsTestamentsCorrect { get; set; }

    public bool ShouldSerializeIsTestamentsCorrect() => IsTestamentsCorrect.HasValue;

    [XmlElement("SF_SignatureSummary", Order = 29)]
    [JsonProperty("SF_SignatureSummary")]
    [JsonPropertyName("SF_SignatureSummary")]
    public string SF_SignatureSummary { get; set; }

    [XmlElement("HasDeceasedSpousePartner", Order = 30)]
    [JsonProperty("HasDeceasedSpousePartner")]
    [JsonPropertyName("HasDeceasedSpousePartner")]
    public bool? HasDeceasedSpousePartner { get; set; }

    public bool ShouldSerializeHasDeceasedSpousePartner() => HasDeceasedSpousePartner.HasValue;

    [XmlElement("DeceasedIsInUndividedEstateClaim", Order = 31)]
    [JsonProperty("DeceasedIsInUndividedEstateClaim")]
    [JsonPropertyName("DeceasedIsInUndividedEstateClaim")]
    public DeceasedIsInUndividedEstateClaim DeceasedIsInUndividedEstateClaim { get; set; }

    [XmlElement("CurrentUserEditProbateChoice", Order = 32)]
    [JsonProperty("CurrentUserEditProbateChoice")]
    [JsonPropertyName("CurrentUserEditProbateChoice")]
    public string CurrentUserEditProbateChoice { get; set; }

    [XmlElement("CurrentUserHasSigned", Order = 33)]
    [JsonProperty("CurrentUserHasSigned")]
    [JsonPropertyName("CurrentUserHasSigned")]
    public bool? CurrentUserHasSigned { get; set; }

    public bool ShouldSerializeCurrentUserHasSigned() => CurrentUserHasSigned.HasValue;

    [XmlElement("SF_CurrentUserSignature", Order = 34)]
    [JsonProperty("SF_CurrentUserSignature")]
    [JsonPropertyName("SF_CurrentUserSignature")]
    public string SF_CurrentUserSignature { get; set; }

  }

  public class TestamentClaims
  {
    [XmlAttribute("altinnRowId")]
    [JsonPropertyName("altinnRowId")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Newtonsoft.Json.JsonIgnore]
    public Guid AltinnRowId { get; set; }

    public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

    [XmlElement("DateOfSignature", Order = 1)]
    [JsonProperty("DateOfSignature")]
    [JsonPropertyName("DateOfSignature")]
    public string DateOfSignature { get; set; }

    [XmlElement("Modified", Order = 2)]
    [JsonProperty("Modified")]
    [JsonPropertyName("Modified")]
    public string Modified { get; set; }

    [XmlElement("SF_ModifiedByName", Order = 3)]
    [JsonProperty("SF_ModifiedByName")]
    [JsonPropertyName("SF_ModifiedByName")]
    public string SF_ModifiedByName { get; set; }

    [XmlElement("ModifiedBy", Order = 4)]
    [JsonProperty("ModifiedBy")]
    [JsonPropertyName("ModifiedBy")]
    public string ModifiedBy { get; set; }

  }

  public class SF_TestamentClaims
  {
    [XmlAttribute("altinnRowId")]
    [JsonPropertyName("altinnRowId")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Newtonsoft.Json.JsonIgnore]
    public Guid AltinnRowId { get; set; }

    public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

    [XmlElement("SF_DateOfSignature", Order = 1)]
    [JsonProperty("SF_DateOfSignature")]
    [JsonPropertyName("SF_DateOfSignature")]
    public string SF_DateOfSignature { get; set; }

  }

  public class MarriagePactClaims
  {
    [XmlAttribute("altinnRowId")]
    [JsonPropertyName("altinnRowId")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Newtonsoft.Json.JsonIgnore]
    public Guid AltinnRowId { get; set; }

    public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

    [XmlElement("DateOfSignature", Order = 1)]
    [JsonProperty("DateOfSignature")]
    [JsonPropertyName("DateOfSignature")]
    public string DateOfSignature { get; set; }

    [XmlElement("Modified", Order = 2)]
    [JsonProperty("Modified")]
    [JsonPropertyName("Modified")]
    public string Modified { get; set; }

    [XmlElement("SF_ModifiedByName", Order = 3)]
    [JsonProperty("SF_ModifiedByName")]
    [JsonPropertyName("SF_ModifiedByName")]
    public string SF_ModifiedByName { get; set; }

    [XmlElement("ModifiedBy", Order = 4)]
    [JsonProperty("ModifiedBy")]
    [JsonPropertyName("ModifiedBy")]
    public string ModifiedBy { get; set; }

  }

  public class SF_MarriagePactClaims
  {
    [XmlAttribute("altinnRowId")]
    [JsonPropertyName("altinnRowId")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Newtonsoft.Json.JsonIgnore]
    public Guid AltinnRowId { get; set; }

    public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

    [XmlElement("SF_DateOfSignature", Order = 1)]
    [JsonProperty("SF_DateOfSignature")]
    [JsonPropertyName("SF_DateOfSignature")]
    public string SF_DateOfSignature { get; set; }

  }

  public class AgriculturalPropertyClaims
  {
    [XmlAttribute("altinnRowId")]
    [JsonPropertyName("altinnRowId")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Newtonsoft.Json.JsonIgnore]
    public Guid AltinnRowId { get; set; }

    public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

    [XmlElement("MunicipalityNumber", Order = 1)]
    [JsonProperty("MunicipalityNumber")]
    [JsonPropertyName("MunicipalityNumber")]
    public string MunicipalityNumber { get; set; }

    [XmlElement("PropertyUnitNumber", Order = 2)]
    [JsonProperty("PropertyUnitNumber")]
    [JsonPropertyName("PropertyUnitNumber")]
    public string PropertyUnitNumber { get; set; }

    [XmlElement("MunicipalityLandNumber", Order = 3)]
    [JsonProperty("MunicipalityLandNumber")]
    [JsonPropertyName("MunicipalityLandNumber")]
    public string MunicipalityLandNumber { get; set; }

    [XmlElement("Modified", Order = 4)]
    [JsonProperty("Modified")]
    [JsonPropertyName("Modified")]
    public string Modified { get; set; }

    [XmlElement("SF_ModifiedByName", Order = 5)]
    [JsonProperty("SF_ModifiedByName")]
    [JsonPropertyName("SF_ModifiedByName")]
    public string SF_ModifiedByName { get; set; }

    [XmlElement("ModifiedBy", Order = 6)]
    [JsonProperty("ModifiedBy")]
    [JsonPropertyName("ModifiedBy")]
    public string ModifiedBy { get; set; }

    [XmlElement("Municipality", Order = 7)]
    [JsonProperty("Municipality")]
    [JsonPropertyName("Municipality")]
    public string Municipality { get; set; }

  }

  public class SF_AgriculturalPropertyClaims
  {
    [XmlAttribute("altinnRowId")]
    [JsonPropertyName("altinnRowId")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Newtonsoft.Json.JsonIgnore]
    public Guid AltinnRowId { get; set; }

    public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

    [XmlElement("SF_MunicipalityNumber", Order = 1)]
    [JsonProperty("SF_MunicipalityNumber")]
    [JsonPropertyName("SF_MunicipalityNumber")]
    public string SF_MunicipalityNumber { get; set; }

    [XmlElement("SF_MunicipalityLandNumber", Order = 2)]
    [JsonProperty("SF_MunicipalityLandNumber")]
    [JsonPropertyName("SF_MunicipalityLandNumber")]
    public string SF_MunicipalityLandNumber { get; set; }

    [XmlElement("SF_PropertyUnitNumber", Order = 3)]
    [JsonProperty("SF_PropertyUnitNumber")]
    [JsonPropertyName("SF_PropertyUnitNumber")]
    public string SF_PropertyUnitNumber { get; set; }

  }

  public class MaritalStatusClaims
  {
    [XmlElement("MaritalStatus", Order = 1)]
    [JsonProperty("MaritalStatus")]
    [JsonPropertyName("MaritalStatus")]
    public string MaritalStatus { get; set; }

    [XmlElement("Modified", Order = 2)]
    [JsonProperty("Modified")]
    [JsonPropertyName("Modified")]
    public string Modified { get; set; }

    [XmlElement("SF_ModifiedByName", Order = 3)]
    [JsonProperty("SF_ModifiedByName")]
    [JsonPropertyName("SF_ModifiedByName")]
    public string SF_ModifiedByName { get; set; }

    [XmlElement("ModifiedBy", Order = 4)]
    [JsonProperty("ModifiedBy")]
    [JsonPropertyName("ModifiedBy")]
    public string ModifiedBy { get; set; }

  }

  public class SF_MaritalStatusClaims
  {
    [XmlElement("MaritalStatus", Order = 1)]
    [JsonProperty("MaritalStatus")]
    [JsonPropertyName("MaritalStatus")]
    public string MaritalStatus { get; set; }

  }

  public class SignatureClaims
  {
    [XmlAttribute("altinnRowId")]
    [JsonPropertyName("altinnRowId")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Newtonsoft.Json.JsonIgnore]
    public Guid AltinnRowId { get; set; }

    public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

    [XmlElement("PartyId", Order = 1)]
    [JsonProperty("PartyId")]
    [JsonPropertyName("PartyId")]
    public string PartyId { get; set; }

    [XmlElement("AcceptsDebt", Order = 2)]
    [JsonProperty("AcceptsDebt")]
    [JsonPropertyName("AcceptsDebt")]
    public bool? AcceptsDebt { get; set; }

    public bool ShouldSerializeAcceptsDebt() => AcceptsDebt.HasValue;

    [XmlElement("Voids60DaysDeadline", Order = 3)]
    [JsonProperty("Voids60DaysDeadline")]
    [JsonPropertyName("Voids60DaysDeadline")]
    public bool? Voids60DaysDeadline { get; set; }

    public bool ShouldSerializeVoids60DaysDeadline() => Voids60DaysDeadline.HasValue;

    [XmlElement("ProbateType", Order = 4)]
    [JsonProperty("ProbateType")]
    [JsonPropertyName("ProbateType")]
    public string ProbateType { get; set; }

    [XmlElement("AcceptsTermsAndConditions", Order = 5)]
    [JsonProperty("AcceptsTermsAndConditions")]
    [JsonPropertyName("AcceptsTermsAndConditions")]
    public bool? AcceptsTermsAndConditions { get; set; }

    public bool ShouldSerializeAcceptsTermsAndConditions() => AcceptsTermsAndConditions.HasValue;

    [XmlElement("Signed", Order = 6)]
    [JsonProperty("Signed")]
    [JsonPropertyName("Signed")]
    public bool? Signed { get; set; }

    public bool ShouldSerializeSigned() => Signed.HasValue;

    [XmlElement("SignedTime", Order = 7)]
    [JsonProperty("SignedTime")]
    [JsonPropertyName("SignedTime")]
    public string SignedTime { get; set; }

    [XmlElement("SF_PartyName", Order = 8)]
    [JsonProperty("SF_PartyName")]
    [JsonPropertyName("SF_PartyName")]
    public string SF_PartyName { get; set; }

    [XmlElement("SubmissionCorrespondenceSent", Order = 9)]
    [JsonProperty("SubmissionCorrespondenceSent")]
    [JsonPropertyName("SubmissionCorrespondenceSent")]
    public bool? SubmissionCorrespondenceSent { get; set; }

    public bool ShouldSerializeSubmissionCorrespondenceSent() => SubmissionCorrespondenceSent.HasValue;

  }

  public class ProbatePrefill
  {
    [XmlElement("Name", Order = 1)]
    [JsonProperty("Name")]
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [XmlElement("Address", Order = 2)]
    [JsonProperty("Address")]
    [JsonPropertyName("Address")]
    public Address Address { get; set; }

    [XmlElement("DateOfDeath", Order = 3)]
    [JsonProperty("DateOfDeath")]
    [JsonPropertyName("DateOfDeath")]
    public string DateOfDeath { get; set; }

    [XmlElement("MaritalStatus", Order = 4)]
    [JsonProperty("MaritalStatus")]
    [JsonPropertyName("MaritalStatus")]
    public string MaritalStatus { get; set; }

    [XmlElement("Heirs", Order = 5)]
    [JsonProperty("Heirs")]
    [JsonPropertyName("Heirs")]
    public List<Heir> Heirs { get; set; }

    [XmlElement("MarriagePactsFromRegister", Order = 6)]
    [JsonProperty("MarriagePactsFromRegister")]
    [JsonPropertyName("MarriagePactsFromRegister")]
    public List<MarriagePactFromRegister> MarriagePactsFromRegister { get; set; }

    [XmlElement("AgriculturalPropertyFromRegister", Order = 7)]
    [JsonProperty("AgriculturalPropertyFromRegister")]
    [JsonPropertyName("AgriculturalPropertyFromRegister")]
    public List<AgriculturalPropertyFromRegister> AgriculturalPropertyFromRegister { get; set; }

  }

  public class Address
  {
    [XmlElement("StreetAddress", Order = 1)]
    [JsonProperty("StreetAddress")]
    [JsonPropertyName("StreetAddress")]
    public string StreetAddress { get; set; }

    [XmlElement("PostalCode", Order = 2)]
    [JsonProperty("PostalCode")]
    [JsonPropertyName("PostalCode")]
    public string PostalCode { get; set; }

    [XmlElement("City", Order = 3)]
    [JsonProperty("City")]
    [JsonPropertyName("City")]
    public string City { get; set; }

  }

  public class Heir
  {
    [XmlAttribute("altinnRowId")]
    [JsonPropertyName("altinnRowId")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Newtonsoft.Json.JsonIgnore]
    public Guid AltinnRowId { get; set; }

    public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

    [XmlElement("Name", Order = 1)]
    [JsonProperty("Name")]
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    [XmlElement("Kinship", Order = 2)]
    [JsonProperty("Kinship")]
    [JsonPropertyName("Kinship")]
    public string Kinship { get; set; }

    [Range(Double.MinValue,Double.MaxValue)]
    [XmlElement("PartyId", Order = 3)]
    [JsonProperty("PartyId")]
    [JsonPropertyName("PartyId")]
    public decimal? PartyId { get; set; }

    public bool ShouldSerializePartyId() => PartyId.HasValue;

  }

  public class MarriagePactFromRegister
  {
    [XmlAttribute("altinnRowId")]
    [JsonPropertyName("altinnRowId")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Newtonsoft.Json.JsonIgnore]
    public Guid AltinnRowId { get; set; }

    public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

    [XmlElement("DateOfSignature", Order = 1)]
    [JsonProperty("DateOfSignature")]
    [JsonPropertyName("DateOfSignature")]
    public string DateOfSignature { get; set; }

    [XmlElement("SpouseFullName", Order = 2)]
    [JsonProperty("SpouseFullName")]
    [JsonPropertyName("SpouseFullName")]
    public string SpouseFullName { get; set; }

  }

  public class AgriculturalPropertyFromRegister
  {
    [XmlAttribute("altinnRowId")]
    [JsonPropertyName("altinnRowId")]
    [System.Text.Json.Serialization.JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Newtonsoft.Json.JsonIgnore]
    public Guid AltinnRowId { get; set; }

    public bool ShouldSerializeAltinnRowId() => AltinnRowId != default;

    [XmlElement("MunicipalityLandNumber", Order = 1)]
    [JsonProperty("MunicipalityLandNumber")]
    [JsonPropertyName("MunicipalityLandNumber")]
    public string MunicipalityLandNumber { get; set; }

    [XmlElement("PropertyUnitNumber", Order = 2)]
    [JsonProperty("PropertyUnitNumber")]
    [JsonPropertyName("PropertyUnitNumber")]
    public string PropertyUnitNumber { get; set; }

    [XmlElement("MunicipalityNumber", Order = 3)]
    [JsonProperty("MunicipalityNumber")]
    [JsonPropertyName("MunicipalityNumber")]
    public string MunicipalityNumber { get; set; }

    [XmlElement("Municipality", Order = 4)]
    [JsonProperty("Municipality")]
    [JsonPropertyName("Municipality")]
    public string Municipality { get; set; }

  }

  public class DeceasedIsInUndividedEstateClaim
  {
    [XmlElement("DeceasedIsInUndividedEstate", Order = 1)]
    [JsonProperty("DeceasedIsInUndividedEstate")]
    [JsonPropertyName("DeceasedIsInUndividedEstate")]
    public string DeceasedIsInUndividedEstate { get; set; }

    [XmlElement("Modified", Order = 2)]
    [JsonProperty("Modified")]
    [JsonPropertyName("Modified")]
    public string Modified { get; set; }

    [XmlElement("ModifiedBy", Order = 3)]
    [JsonProperty("ModifiedBy")]
    [JsonPropertyName("ModifiedBy")]
    public string ModifiedBy { get; set; }

    [XmlElement("SF_ModifiedBy", Order = 4)]
    [JsonProperty("SF_ModifiedBy")]
    [JsonPropertyName("SF_ModifiedBy")]
    public string SF_ModifiedBy { get; set; }

  }
}
