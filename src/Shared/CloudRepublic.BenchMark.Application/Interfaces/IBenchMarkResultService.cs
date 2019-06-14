using System.Collections.Generic;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Domain.Entities;

namespace CloudRepublic.BenchMark.Application.Interfaces
{
    public interface IBenchMarkResultService
    {
        Task<IEnumerable<BenchMarkResult>> GetBenchMarkResults(string cloudProvider,string hostingEnvironment, string runtime,int dayRange);
    }
}