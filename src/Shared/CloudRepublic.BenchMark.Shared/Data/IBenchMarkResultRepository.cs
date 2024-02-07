using System.Collections.Generic;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;

namespace CloudRepublic.BenchMark.Data;

public interface IBenchMarkResultRepository
{
    public Task AddBenchMarkResultAsync(BenchMarkResult benchMarkResult);

    public Task<IEnumerable<BenchMarkResult>> GetBenchMarkResultsAsync(
        CloudProvider provider, 
        HostEnvironment environment,
        Runtime runtime,
        Language language, 
        string sku,
        int year, 
        int month,
        int day
        );
}