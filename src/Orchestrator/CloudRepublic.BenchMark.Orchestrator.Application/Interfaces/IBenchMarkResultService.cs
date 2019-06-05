using System.Collections.Generic;
using System.Threading.Tasks;
using CloudRepublic.BenchMark.Orchestrator.Domain.Enums;

namespace CloudRepublic.BenchMark.Orchestrator.Application.Interfaces
{
    public interface IBenchMarkResultService
    {
        Task StoreResults(CloudProvider cloudProvider, HostEnvironment hostEnvironment, Runtime runtime,
            IEnumerable<long?> results);
    }
}