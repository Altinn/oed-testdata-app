using oed_testdata.Server.Infrastructure.TestdataStore.Bank;

namespace oed_testdata.Server.Infrastructure.TestdataStore.Svv
{
    public class SvvFileStore(ILogger<BankFileStore> logger) : FileStore, ISvvStore
    {
        private const string BasePath = "./Testdata/Json/Svv";

        public async Task<SvvResponse> GetVehicles(int partyId)
        {
            EnsureDirectory(BasePath);

            var response = await GetForParty<SvvResponse>(BasePath, partyId);
            if (response is not null)
            {
                logger.LogInformation("Returning SPECIFIC vehicle testdata for partyId [{partyId}]", partyId);
                return response;
            }

            logger.LogInformation("Returning DEFAULT vehicle testdata for partyId [{partyId}]", partyId);
            return await GetDefault<SvvResponse>(BasePath);
        }
    }
}
