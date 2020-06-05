using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudRepublic.BenchMark.Orchestrator.Infrastructure
{
    public interface IBenchMarkTypeService
    {
        IEnumerable<BenchMarkType> GetAllTypes();
        Task StoreBenchMarkResultsAsync(IEnumerable<BenchMarkResult> results);
        /// <summary>
        /// This function will first run all 'Cold' BenchMarks, than wait the delay, and than run all 'Warm' BenchMarks
        /// </summary>
        /// <param name="benchMarkTypes">BenchMark Types/Options to run for</param>
        /// <param name="coldCalls">amount of cold calls to do</param>
        /// <param name="warmCalls">Amount of warm calls to do, please note that with no <see cref="coldCalls"></see> these will not become 'Warm'</param>
        /// <param name="delayBetweenCalls">Time to wait before starting the 'Warm' calls after making all 'Cold' calls</param>
        /// <returns></returns>
        Task<List<BenchMarkResult>> RunBenchMarksAsync(IEnumerable<BenchMarkType> benchMarkTypes, int coldCalls = 5, int warmCalls = 10, int delayBetweenCalls = 30);
    }
}
