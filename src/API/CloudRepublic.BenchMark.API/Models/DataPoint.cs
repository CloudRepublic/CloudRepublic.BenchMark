using System;

namespace CloudRepublic.BenchMark.API.Models
{
    public class DataPoint
    {
        public DataPoint(string createdAt, int executionTime)
        {
            CreatedAt = createdAt;
            ExecutionTime = executionTime;
        }
        public string CreatedAt { get;  }
        public int ExecutionTime { get;  }
    }
}