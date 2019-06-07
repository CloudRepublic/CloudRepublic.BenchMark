using System.Threading.Tasks;
using CloudRepublic.BenchMark.Orchestrator.Application.Models;

namespace CloudRepublic.BenchMark.Orchestrator.Application.Interfaces
{
    public interface IBenchMarkService
    {
        Task<BenchMarkResponse> RunBenchMark(BenchMarkType benchMarkType);
    }
}