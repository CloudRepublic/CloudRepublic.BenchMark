using System;

namespace CloudRepublic.BenchMark.API.Models
{
    public class ResponseModel
    {
        public DateTime UpdatedAt { get; set; }
        public BenchMarkData Data { get; set; }
    }
}