using System.Collections.Generic;

namespace CloudRepublic.BenchMark.API.Models
{
    public class Runtime
    {
        public string Name { get; set; }
        public int AverageExecutionTime { get; set; }
        public List<DataPoint> DataPoints { get; set; }
    }
}