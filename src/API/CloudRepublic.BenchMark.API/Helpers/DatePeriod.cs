using System;

namespace CloudRepublic.BenchMark.API.Helpers
{
    public class DatePeriod
    {
        public DatePeriod(DateTime end, DateTime start)
        {
            StartDate = start;
            EndDate = end;
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ValidPeriod
        {
            get
            {
                return (StartDate != null && EndDate != null && EndDate <= StartDate);
            }
        }
    }
}
