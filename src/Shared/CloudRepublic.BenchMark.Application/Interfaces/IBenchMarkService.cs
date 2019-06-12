using System.Threading.Tasks;
using CloudRepublic.BenchMark.Application.Models;

namespace CloudRepublic.BenchMark.Application.Interfaces
{
    public interface IBenchMarkService
    {
        Task<BenchMarkResponse> RunBenchMark(BenchMarkType benchMarkType);
    }
}