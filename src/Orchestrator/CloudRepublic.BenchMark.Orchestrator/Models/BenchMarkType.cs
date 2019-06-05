using CloudRepublic.BenchMark.Orchestrator.Domain.Enums;

namespace CloudRepublic.BenchMark.Orchestrator.Models
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