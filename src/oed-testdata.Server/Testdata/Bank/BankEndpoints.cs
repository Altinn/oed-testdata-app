using oed_testdata.Server.Infrastructure.TestdataStore.Bank;

namespace oed_testdata.Server.Testdata.Bank;

public static class BankEndpoints
{
    private const string HandlingCallMessage = "Handling call for {Path}";

    public static void MapBankEndpoints(this WebApplication app)
    {
        app
            .MapGroup("/api/testdata/externalapi/bank/{instanceOwnerPartyId:int}/{instanceGuid:guid}")
            .MapEndpoints()
            .AllowAnonymous();
    }

    private static RouteGroupBuilder MapEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetBankCustomerRelations);
        group.MapGet("/{bankOrgNo}", GetBankDetails);
        // The transaction endpoints return the same file whatever bank or account is asked for,
        // so their route parameters are deliberately left unbound.
#pragma warning disable ASP0018
        group.MapGet("/{bankOrgNo}/transactions", GetTransactionsFile);
        group.MapPost("/{bankOrgNo}/transactions/{accountRefNo}", GetTransactionsFile);
#pragma warning restore ASP0018
        return group;
    }

    private static async Task<IResult> GetBankCustomerRelations(
        int instanceOwnerPartyId,
        HttpContext httpContext,
        IBankStore bankStore,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(BankEndpoints));
        logger.LogInformation(HandlingCallMessage, httpContext.Request.Path.Value);

        var resp = await bankStore.GetBankRelations(instanceOwnerPartyId);
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
        logger.LogInformation(HandlingCallMessage, httpContext.Request.Path.Value);

        var resp = await bankStore.GetBankDetails(instanceOwnerPartyId, bankOrgNo);
        return Results.Ok(resp);
    }

    private static async Task<IResult> GetTransactionsFile(
        HttpContext httpContext,
        IBankStore bankStore,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(typeof(BankEndpoints));
        logger.LogInformation(HandlingCallMessage, httpContext.Request.Path.Value);

        var resp = await bankStore.GetAccountTransactionsFile();
        return Results.File(resp, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Transaksjonshistorikk.xlsx");
    }
}