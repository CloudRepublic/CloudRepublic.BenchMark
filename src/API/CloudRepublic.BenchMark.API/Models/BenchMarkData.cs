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
        public string Language { get; set; }
        public bool ColdPreviousDayPositive { get; set; }
        public double ColdPreviousDayDifference { get; set; }
        public bool WarmPreviousDayPositive { get; set; }
        public double WarmPreviousDayDifference { get; set; }
        public double ColdMedianExecutionTime { get; set; }
        public double WarmMedianExecutionTime { get; set; }
        public List<DataPoint> ColdDataPoints { get; set; }
        public List<DataPoint> WarmDataPoints { get; set; }
    }
}