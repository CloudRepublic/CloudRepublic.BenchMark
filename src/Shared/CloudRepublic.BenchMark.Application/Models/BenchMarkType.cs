using CloudRepublic.BenchMark.Domain.Enums;

namespace CloudRepublic.BenchMark.Application.Models
{
    public class BenchMarkType
    {
        public CloudProvider CloudProvider { get; set; }
        public HostEnvironment HostEnvironment { get; set; }
        public Runtime Runtime { get; set; }

        /// <summary>
        /// this is actually the: CloudProviderHostEnvironmentRuntime as string.
        ///  the identifier for this Benchmark, this is used to designate the Client,  Url and Key.
        /// </summary>
        public string Name { get; set; }
        public string ClientName { get { return Name + "Client"; } }
        public string UrlName { get { return Name + "Url"; } }
        public string KeyName { get { return Name + "Key"; } }

        /// <summary>
        /// Whether an x-functions-key with the Keyname will be added to the Http client default request
        /// </summary>
        public bool SetXFunctionsKey { get; set; } = true;
    }
}