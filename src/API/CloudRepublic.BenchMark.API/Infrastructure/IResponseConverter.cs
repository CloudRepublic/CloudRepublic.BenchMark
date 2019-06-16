using System.Collections.Generic;
using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Domain.Entities;

namespace CloudRepublic.BenchMark.API.Infrastructure
{
    public interface IResponseConverter
    {
        BenchMarkData ConvertToBenchMarkData(List<BenchMarkResult> resultDataPoints);
    }
}