using System.Text.Json.Serialization;
// ReSharper disable InconsistentNaming
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace oed_testdata.Server.Infrastructure.TestdataStore;

public class BankCustomerRelation
{
    [JsonPropertyName("orgNo")] public string? OrgNo { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("url")] public object? Url { get; set; }
    [JsonPropertyName("version")] public string? Version { get; set; }
    [JsonPropertyName("env")] public string? Env { get; set; }
}


public class BankResponse
{
    public Bankaccount[]? bankAccounts { get; set; }
}

public class Bankaccount
{
    public string bankName { get; set; }
    public bool isImplemented { get; set; }
    public bool hasErrors { get; set; }
    public Account[] accounts { get; set; }
}

public class Account
{
    public string accountNumber { get; set; }
    public Accountdetail accountDetail { get; set; }
    public object transactions { get; set; }
    public int accountAvailableBalance { get; set; }
    public int accountBookedBalance { get; set; }
}

public class Accountdetail
{
    public string status { get; set; }
    public Servicer servicer { get; set; }
    public string accountIdentifier { get; set; }
    public string accountReference { get; set; }
    public string type { get; set; }
    public string currency { get; set; }
    public string name { get; set; }
    public Balance[] balances { get; set; }
    public Primaryowner primaryOwner { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
}

public class Servicer
{
    public Identifier identifier { get; set; }
    public string name { get; set; }
}

public class Identifier
{
    public string countryOfResidence { get; set; }
    public string value { get; set; }
    public string type { get; set; }
}

public class Primaryowner
{
    public string permission { get; set; }
    public Identifier1 identifier { get; set; }
    public string name { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
}

public class Identifier1
{
    public string value { get; set; }
    public string type { get; set; }
}

public class Balance
{
    public bool creditLineIncluded { get; set; }
    public int amount { get; set; }
    public string creditDebitIndicator { get; set; }
    public DateTime registered { get; set; }
    public string type { get; set; }
    public int creditLineAmount { get; set; }
    public string creditLineCurrency { get; set; }
    public string currency { get; set; }
}
