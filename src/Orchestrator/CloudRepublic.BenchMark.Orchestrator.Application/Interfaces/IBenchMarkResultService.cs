using System.Collections.Generic;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Orchestrator.Domain.Entities;

namespace CloudRepublic.BenchMark.Orchestrator.Application.Interfaces
{
    public interface IBenchMarkResultService
    {
        Task<IEnumerable<BenchMarkResult>> GetBenchMarkResults(int dayRange);
    }
}