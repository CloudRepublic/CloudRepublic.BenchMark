using System.Collections.Generic;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;

namespace CloudRepublic.BenchMark.Data;

public interface IBenchMarkResultRepository
{
    public Task AddBenchMarkResultAsync(BenchMarkResult benchMarkResult);

    public IAsyncEnumerable<BenchMarkResult> GetBenchMarkResultsAsync(
        CloudProvider provider, 
        HostEnvironment environment,
        Runtime runtime,
        Language language, 
        int year, 
        int month
        );
}