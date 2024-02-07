using CloudRepublic.BenchMark.Application.Models;

namespace CloudRepublic.BenchMark.Orchestrator.V2.Interfaces;

public interface IBenchMarkService
{
    /// <summary>
    /// Starts the client and makes the Request
    /// </summary>
    /// <param name="clientName"></param>
    /// <returns></returns>
    Task<BenchMarkResponse> RunBenchMarkAsync(BenchMarkType benchMarkType, int CallPositionNumber);
}