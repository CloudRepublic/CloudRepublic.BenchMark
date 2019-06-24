using System.Collections.Generic;

namespace CloudRepublic.BenchMark.API.Models
{
    public class BenchMarkDataSet
    {
        public string DateLabel { get; set; }

        public List<DataPoint> DataPoints { get; set; }
    }
}