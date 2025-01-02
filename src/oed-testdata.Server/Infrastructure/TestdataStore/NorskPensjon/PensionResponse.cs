namespace oed_testdata.Server.Infrastructure.TestdataStore.NorskPensjon
{
    public class PensionResponse
    {
        public List<InsurancePolicy> InsurancesPolicies { get; set; }
    }

    public class InsurancePolicy
    {
        public DateTimeOffset DisclosureDate { get; set; }

        public string Url { get; set; }

        public string ProductType { get; set; }

        public string PensionScheme { get; set; }

        public string Reference { get; set; }

        public string Description { get; set; }
    }
}
