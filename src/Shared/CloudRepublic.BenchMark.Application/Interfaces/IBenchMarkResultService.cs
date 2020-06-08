using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudRepublic.BenchMark.Application.Interfaces
{
    public interface IBenchMarkResultService
    {
        Task<List<BenchMarkResult>> GetBenchMarkResults(CloudProvider cloudProvider, HostEnvironment hostingEnvironment, Runtime runtime, int dayRange);
    }
}