using CloudRepublic.BenchMark.Domain.Enums;

namespace CloudRepublic.BenchMark.Application.Models
{
    public class BenchMarkType
    {
        public CloudProvider CloudProvider { get; }
        public HostEnvironment HostEnvironment { get; }
        public Runtime Runtime { get; }

        public BenchMarkType(CloudProvider cloudProvider, HostEnvironment hostEnvironment, Runtime runtime)
        {
            CloudProvider = cloudProvider;
            HostEnvironment = hostEnvironment;
            Runtime = runtime;
        }
    }
}