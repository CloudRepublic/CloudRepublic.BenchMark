using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using System.Collections.Generic;

namespace CloudRepublic.BenchMark.API.Interfaces
{
    public interface IResponseConverterService
    {
        List<BenchMarkOption> ConvertToBenchMarkOptions(List<BenchMarkType> benchMarkTypes);
        BenchMarkData ConvertToBenchMarkData(List<BenchMarkResult> resultDataPoints);
    }
}