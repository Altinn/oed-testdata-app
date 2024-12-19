using System.Text.Json;

namespace oed_testdata.Server.Infrastructure.TestdataStore;

public class BankFileStore(ILogger<BankFileStore> logger) : IBankStore
{
    private const string CustomerRelationsPath = "./Testdata/Json/Bank/CustomerRelations";
    private const string BankDetailsPath = "./Testdata/Json/Bank/BankDetails";
    private const string TransactionsPath = "./Testdata/Json/Bank/Transactions";

    public async Task<IEnumerable<BankCustomerRelation>> GetCustomerRelations(int partyId)
    {
        EnsureDirectory(CustomerRelationsPath);
        
        var response = await GetSpecificCustomerRelations(partyId);
        if (response is not null)
        {
            logger.LogInformation("Returning SPECIFIC customer relations testdata for partyId [{partyId}]", partyId);
            return response;
        }

        logger.LogInformation("Returning DEFAULT customer relations testdata for partyId [{partyId}]", partyId);
        return await GetDefaultCustomerRelations();
    }

    public async Task<BankResponse> GetBankDetails(int partyId, string bankOrgNo)
    {
        EnsureDirectory(BankDetailsPath);

        var response = await GetSpecificBankDetails(partyId, bankOrgNo);
        if (response is not null)
        {
            logger.LogInformation("Returning SPECIFIC bank details testdata for partyId [{partyId}], bankOrgNo [{bankOrgNo}]", partyId, bankOrgNo);
            return response;
        }

        logger.LogInformation("Returning DEFAULT bank details testdata for partyId [{partyId}], bankOrgNo [{bankOrgNo}]", partyId, bankOrgNo);
        return await GetDefaultBankDetails();
    }

    public async Task<byte[]> GetAccountTransactionsFile(int partyId, string bankOrgNo, string accountRef)
    {
        EnsureDirectory(TransactionsPath);

        var file = Path.Combine(TransactionsPath, "Transaksjonshistorikk.xlsx");
        if (!File.Exists(file))
            return [];

        var fileBytes = await File.ReadAllBytesAsync(file);
        return fileBytes ?? [];
    }

    private static async Task<IEnumerable<BankCustomerRelation>?> GetSpecificCustomerRelations(int partyId)
    {
        EnsureDirectory(CustomerRelationsPath);

        var files = Directory.EnumerateFiles(CustomerRelationsPath);
        var file = files.SingleOrDefault(f => f.Contains(partyId.ToString()));

        if (file is null) return null;

        await using var fileStream = File.OpenRead(file);
        var data = await JsonSerializer.DeserializeAsync<List<BankCustomerRelation>>(fileStream);

        return data;
    }

    private static async Task<IEnumerable<BankCustomerRelation>> GetDefaultCustomerRelations()
    {
        var file = Path.Combine(CustomerRelationsPath, "default.json");
        if (!File.Exists(file))
            return [];

        await using var fileStream = File.OpenRead(file);
        var data = await JsonSerializer.DeserializeAsync<IEnumerable<BankCustomerRelation>>(fileStream);

        return data ?? [];
    }

    private static async Task<BankResponse?> GetSpecificBankDetails(int partyId, string bankOrgNo)
    {
        var prefix = $"{partyId}-{bankOrgNo}";
        var files = Directory.EnumerateFiles(BankDetailsPath);
        var file = files.SingleOrDefault(f => f.Contains(prefix));

        if (file is null) return null;

        await using var fileStream = File.OpenRead(file);
        var data = await JsonSerializer.DeserializeAsync<BankResponse>(fileStream);

        return data;
    }

    private static async Task<BankResponse> GetDefaultBankDetails()
    {
        var file = Path.Combine(BankDetailsPath, "default.json");
        if (!File.Exists(file))
            return new BankResponse();

        await using var fileStream = File.OpenRead(file);
        var data = await JsonSerializer.DeserializeAsync<BankResponse>(fileStream);

        return data ?? new BankResponse();
    }


    private static void EnsureDirectory(string path)
    {
        if (!Directory.Exists(path))
            throw new Exception("No path");
    }
}