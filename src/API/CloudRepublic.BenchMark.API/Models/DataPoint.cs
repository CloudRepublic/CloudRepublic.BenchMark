using System;

namespace CloudRepublic.BenchMark.API.Models
{
    public class DataPoint
    {
        public DataPoint(DateTimeOffset createdAt, int executionTime)
        {
            CreatedAt = createdAt;
            ExecutionTime = executionTime;
        }
        public DateTimeOffset CreatedAt { get;  }
        public int ExecutionTime { get;  }
    }
}