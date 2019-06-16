using System.Collections.Generic;

namespace CloudRepublic.BenchMark.API.Models
{
    public class CloudProvider
    {
        public string Name { get; set; }
        public HostingEnvironment HostingEnvironments { get; set; }
    }
}