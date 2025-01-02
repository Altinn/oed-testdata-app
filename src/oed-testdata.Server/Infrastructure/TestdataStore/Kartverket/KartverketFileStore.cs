namespace oed_testdata.Server.Infrastructure.TestdataStore.Kartverket
{
    public class KartverketFileStore(ILogger<KartverketFileStore> logger) : FileStore, IKartverketStore
    {
        private const string BasePath = "./Testdata/Json/Kartverket";

        public async Task<KartverketResponse> GetProperties(int partyId)
        {
            var response = await GetForParty<KartverketResponse>(BasePath, partyId);
            if (response is not null)
            {
                logger.LogInformation("Returning SPECIFIC property testdata for partyId [{partyId}]", partyId);
                return response;
            }

            logger.LogInformation("Returning DEFAULT property testdata for partyId [{partyId}]", partyId);
            return await GetDefault<KartverketResponse>(BasePath);

        }
    }
}
