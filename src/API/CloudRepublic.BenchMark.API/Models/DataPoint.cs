using System;
using System.Collections.Generic;

namespace CloudRepublic.BenchMark.API.Models
{
    public class DataPoint
    {
        public DataPoint(string createdAt, List<int> executionTimes)
        {
            CreatedAt = createdAt;
            ExecutionTimes = executionTimes;
        }
        public string CreatedAt { get;  }
        public List<int> ExecutionTimes { get;  }
    }
}