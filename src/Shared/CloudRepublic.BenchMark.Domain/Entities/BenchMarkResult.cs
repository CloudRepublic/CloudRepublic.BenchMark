using CloudRepublic.BenchMark.Domain.Enums;
using System;

namespace CloudRepublic.BenchMark.Domain.Entities
{
    public partial class BenchMarkResult
    {
        public int Id { get; set; }
        public CloudProvider CloudProvider { get; set; }
        public HostEnvironment HostingEnvironment { get; set; }
        public Language Language { get; set; }
        public AzureRuntimeVersion AzureRuntimeVersion { get; set; }
        public bool Success { get; set; }
        public int RequestDuration { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        
        public bool IsColdRequest { get; set; }
    }
}
