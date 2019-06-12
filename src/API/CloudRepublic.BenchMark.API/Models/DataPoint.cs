using System;

namespace CloudRepublic.BenchMark.API.Models
{
    public class DataPoint
    {
        public DateTimeOffset CreatedAt { get; set; }
        public int ExecutionTime { get; set; }
    }
}