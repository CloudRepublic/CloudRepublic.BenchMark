using CloudRepublic.BenchMark.Domain.Enums;

namespace CloudRepublic.BenchMark.Application.Models
{
    public class BenchMarkType
    {
        public CloudProvider CloudProvider { get; set; }
        public HostEnvironment HostEnvironment { get; set; }
        public Runtime Runtime { get; set; }
        public Language Language { get; set; }
        public string Sku { get; set; }

        public string Title { get; set; }
        
        /// <summary>
        /// this is actually the: CloudProviderHostEnvironmentRuntime as string.
        ///  the identifier for this Benchmark, this is used to designate the Client,  Url and Key.
        /// </summary>
        public string Name { get; set; }
        public string TestEndpoint { get; set; }
        public string AuthenticationHeaderName { get; set; }
        public string AuthenticationHeaderValue { get; set; }
        public int SortOrder { get; set; }
    }
}