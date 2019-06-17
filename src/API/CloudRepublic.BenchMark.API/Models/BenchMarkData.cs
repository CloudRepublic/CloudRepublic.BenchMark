using System.Collections.Generic;

namespace CloudRepublic.BenchMark.API.Models
{
    public class BenchMarkData
    {
        public BenchMarkData()
        {
            ColdDataPoints = new List<DataPoint>();
            WarmDataPoints = new List<DataPoint>();
        }
        public string CloudProvider { get; set; }
        public string HostingEnvironment { get; set; }
        public string Runtime { get; set; }
        public int AverageExecutionTime { get; set; }
        public List<DataPoint> ColdDataPoints { get; set; }
        public List<DataPoint> WarmDataPoints { get; set; }
    }
}