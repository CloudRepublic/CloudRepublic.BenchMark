using CloudRepublic.BenchMark.Domain.Enums;

namespace CloudRepublic.BenchMark.Application.Models
{
    public class BenchMarkType
    {
        public CloudProvider CloudProvider { get; set; }
        public HostEnvironment HostEnvironment { get; set; }
        public Runtime Runtime { get; set; }
        public string ClientName { get; set; }
        public BenchMarkType() { }

        public BenchMarkType(CloudProvider cloudProvider, HostEnvironment hostEnvironment, Runtime runtime)
        {
            CloudProvider = cloudProvider;
            HostEnvironment = hostEnvironment;
            Runtime = runtime;
        }
    }
}