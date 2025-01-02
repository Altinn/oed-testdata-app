using oed_testdata.Server.Testdata.Bank;
using oed_testdata.Server.Testdata.Ektepakt;
using oed_testdata.Server.Testdata.Estate;
using oed_testdata.Server.Testdata.Kartverket;
using oed_testdata.Server.Testdata.NorskPensjon;
using oed_testdata.Server.Testdata.Svv;

namespace oed_testdata.Server.Testdata
{
    public static class WebApplicationExtensions
    {
        public static void MapTestdataEndpoints(this WebApplication app)
        {
            app.MapEstateEndpoints();
            app.MapBankEndpoints();
            app.MapSvvEndpoints();
            app.MapKartverketEndpoints();
            app.MapEktepaktEndpoints();
            app.MapNorskPensjonEndpoints();
        }
    }
}
