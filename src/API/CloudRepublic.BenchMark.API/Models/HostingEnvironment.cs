using System.Collections.Generic;

namespace CloudRepublic.BenchMark.API.Models
{
    public class HostingEnvironment
    {
        public string Name { get; set; }
        public List<Runtime> Runtimes { get; set; }
    }
}