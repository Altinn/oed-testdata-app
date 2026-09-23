using oed_testdata.Server.Infrastructure.TestdataStore.Estate;

namespace oed_testdata.Server.Services;

public class EstateResetBackgroundService(
    ILogger<EstateResetBackgroundService> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private static readonly TimeZoneInfo OsloTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Oslo");
    private static readonly TimeOnly RunAt = new(3, 0);
    private const string AutoResetTag = "Auto-reset";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("BackgroundService starting");

        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = GetDelayUntilNextRun(DateTimeOffset.UtcNow);
            logger.LogInformation("Next estate reset in {Delay}", delay);

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            try
            {
                await ResetEstates();
            }
            catch (Exception ex)
            {
                // Catching here keeps one failed night from stopping the host
                logger.LogError(ex, "An error occurred while executing the background service.");
            }
        }

        logger.LogInformation("BackgroundService terminated");
    }

    private async Task ResetEstates()
    {
        logger.LogInformation("EstateResetBackgroundService running at: {Time}", DateTimeOffset.Now);

        using var scope = scopeFactory.CreateScope();
        var store = scope.ServiceProvider.GetRequiredService<IEstateStore>();
        var estateService = scope.ServiceProvider.GetRequiredService<IEstateService>();

        var allEstates = await store.ListAll();
        var estatesToReset = allEstates
            .Where(e => e.Metadata.Tags.Contains(AutoResetTag))
            .Select(e => e.EstateSsn)
            .ToList();

        foreach (var ssn in estatesToReset)
        {
            try
            {
                await estateService.CreateOrRecreateEstate(ssn, null);
                logger.LogInformation("Reset estate with SSN: {EstateSsn}", ssn);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to reset estate with SSN: {EstateSsn}", ssn);
            }
        }
    }

    private static TimeSpan GetDelayUntilNextRun(DateTimeOffset utcNow)
    {
        var localNow = TimeZoneInfo.ConvertTime(utcNow, OsloTimeZone);
        var nextLocal = localNow.Date.Add(RunAt.ToTimeSpan());
        if (nextLocal <= localNow.DateTime)
            nextLocal = nextLocal.AddDays(1);

        var nextUtc = TimeZoneInfo.ConvertTimeToUtc(nextLocal, OsloTimeZone);
        return nextUtc - utcNow.UtcDateTime;
    }
}
