namespace oed_testdata.Server.Infrastructure.TestdataStore.Ektepakt;

public class EktepaktFileStore(ILogger<EktepaktFileStore> logger) : FileStore, IEktepaktStore
{
    private const string BasePath = "./Testdata/Json/Ektepakt";

    public async Task<EktepaktResponse> GetEktepakter(int partyId)
    {
        var response = await GetForParty<EktepaktResponse>(BasePath, partyId);
        if (response is not null)
        {
            logger.LogInformation("Returning SPECIFIC ektepakt testdata for partyId [{partyId}]", partyId);
            return response;
        }

        logger.LogInformation("Returning DEFAULT ektepakt testdata for partyId [{partyId}]", partyId);
        return await GetDefault<EktepaktResponse>(BasePath);
    }
}