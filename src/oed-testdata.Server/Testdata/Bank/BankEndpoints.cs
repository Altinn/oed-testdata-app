using oed_testdata.Server.Infrastructure.TestdataStore.Bank;

namespace oed_testdata.Server.Testdata.Bank;

public static class BankEndpoints
{
    public static void MapBankEndpoints(this WebApplication app)
    {
        app
            .MapGroup("/api/testdata/banks/{instanceOwnerPartyId:int}/{instanceGuid:guid}")
            .MapEndpoints()
            .AllowAnonymous();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetBankCustomerRelations);
        group.MapGet("/{bankOrgNo}", GetBankDetails);
        group.MapGet("/{bankOrgNo}/transactions", GetBankTransactions);
        group.MapPost("/{bankOrgNo}/transactions/{accountRefNo}", GetAccountTransactions);
        return group;
    }

    private static async Task<IResult> GetBankCustomerRelations(
        int instanceOwnerPartyId,
        HttpContext httpContext,
        IBankStore bankStore,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(BankEndpoints));
        logger.LogInformation("Handling call for {path}", httpContext.Request.Path.Value);

        var resp = await bankStore.GetCustomerRelations(instanceOwnerPartyId);
        return Results.Ok(resp);
    }
    
    private static async Task<IResult> GetBankDetails(
        int instanceOwnerPartyId,
        string bankOrgNo,
        HttpContext httpContext,
        IBankStore bankStore,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(BankEndpoints));
        logger.LogInformation("Handling call for {path}", httpContext.Request.Path.Value);

        var resp = await bankStore.GetBankDetails(instanceOwnerPartyId, bankOrgNo);
        return Results.Ok(resp);
    }

    private static async Task<IResult> GetAccountTransactions(
        int instanceOwnerPartyId,
        string bankOrgNo,
        string accountRefNo,
        HttpContext httpContext,
        IBankStore bankStore,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(BankEndpoints));
        logger.LogInformation("Handling call for {path}", httpContext.Request.Path.Value);

        var resp = await bankStore.GetAccountTransactionsFile();
        return Results.File(resp, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Transaksjonshistorikk.xlsx");
    }

    private static async Task<IResult> GetBankTransactions(
        int instanceOwnerPartyId,
        string bankOrgNo,
        HttpContext httpContext,
        IBankStore bankStore,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(BankEndpoints));
        logger.LogInformation("Handling call for {path}", httpContext.Request.Path.Value);

        var resp = await bankStore.GetAccountTransactionsFile();
        return Results.File(resp, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Transaksjonshistorikk.xlsx");
    }
}