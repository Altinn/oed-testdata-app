using oed_testdata.Server.Infrastructure.Maskinporten.Models;
using oed_testdata.Server.Infrastructure.OedEvents;
using oed_testdata.Server.Infrastructure.TestdataStore;

namespace oed_testdata.Server.Infrastructure.Oed
{
    public interface IOedClien
    {
        public Task<Declaration> GetDeclaration();
    }

    public class OedClient: IOedClient
    {
        public Task PostDaEvent(DaData data)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOedInstance(string partyId, string oedInstanceGuid)
        {
            throw new NotImplementedException();
        }

        public Task DeleteOedDeclarationInstance(string partyId, string oedDeclarationInstanceGuid)
        {
            throw new NotImplementedException();
        }
    }
}
