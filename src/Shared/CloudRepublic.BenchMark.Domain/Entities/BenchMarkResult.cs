using System;

namespace CloudRepublic.BenchMark.Domain.Entities
{
    public partial class BenchMarkResult
    {
       
        public int Id { get; set; }
        public int CloudProvider { get; set; }
        public int HostingEnvironment { get; set; }
        public int Runtime { get; set; }
        public bool Success { get; set; }
        public int RequestDuration { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
