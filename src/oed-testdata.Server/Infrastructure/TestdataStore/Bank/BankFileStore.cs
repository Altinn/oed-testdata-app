using System.Text.Json;

namespace oed_testdata.Server.Infrastructure.TestdataStore.Bank;

public class BankFileStore(ILogger<BankFileStore> logger) : FileStore, IBankStore
{
    private const string CustomerRelationsPath = "./Testdata/Json/Bank/CustomerRelations";
    private const string BankDetailsPath = "./Testdata/Json/Bank/BankDetails";
    private const string TransactionsPath = "./Testdata/Json/Bank/Transactions";
    private const string AllInOnePath = "./Testdata/Json/Bank/AllInOne";

    public async Task<BankResponse> GetAllInOne(int partyId)
    {
        var response = await GetForParty<BankResponse>(AllInOnePath, partyId);
        if (response is not null)
        {
            logger.LogInformation("Returning SPECIFIC all-in-one testdata for partyId [{partyId}]", partyId);
            return response;
        }

        logger.LogInformation("Returning DEFAULT all-in-one testdata for partyId [{partyId}]", partyId);
        return await GetDefault<BankResponse>(AllInOnePath);

    }

    public async Task<IEnumerable<BankCustomerRelation>> GetCustomerRelations(int partyId)
    {
        var response = await GetForParty<List<BankCustomerRelation>>(CustomerRelationsPath, partyId);
        if (response is not null)
        {
            logger.LogInformation("Returning SPECIFIC customer relations testdata for partyId [{partyId}]", partyId);
            return response;
        }

        logger.LogInformation("Returning DEFAULT customer relations testdata for partyId [{partyId}]", partyId);
        return await GetDefault<List<BankCustomerRelation>>(CustomerRelationsPath);
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
        return await GetDefault<BankResponse>(BankDetailsPath);
    }

    public async Task<byte[]> GetAccountTransactionsFile()
    {
        EnsureDirectory(TransactionsPath);

        var file = Path.Combine(TransactionsPath, "Transaksjonshistorikk.xlsx");
        if (!File.Exists(file))
            return [];

        var fileBytes = await File.ReadAllBytesAsync(file);
        return fileBytes;
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
}