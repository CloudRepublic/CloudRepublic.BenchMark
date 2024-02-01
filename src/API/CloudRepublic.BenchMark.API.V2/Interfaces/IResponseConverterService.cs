using CloudRepublic.BenchMark.API.V2.Models;
using CloudRepublic.BenchMark.Domain.Entities;

namespace CloudRepublic.BenchMark.API.V2.Interfaces
{
    public interface IResponseConverterService
    {
        BenchMarkData ConvertToBenchMarkData(List<BenchMarkResult> resultDataPoints);
    }
}