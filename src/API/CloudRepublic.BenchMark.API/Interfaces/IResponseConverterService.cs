using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using System.Collections.Generic;

namespace CloudRepublic.BenchMark.API.Interfaces
{
    public interface IResponseConverterService
    {
        BenchMarkData ConvertToBenchMarkData(List<BenchMarkResult> resultDataPoints);
    }
}