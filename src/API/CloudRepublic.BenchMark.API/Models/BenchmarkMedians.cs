using System;

namespace CloudRepublic.BenchMark.API.Models
{
    public class BenchmarkMedians
    {
        public double CurrentDay;
        public double PreviousDay;
        public double Difference
        {
            get
            {
                return CurrentDay == 0 ? 0 : Math.Round(((CurrentDay - PreviousDay) / Math.Abs(CurrentDay)) * 100, 2);
            }
        }
    }
}
