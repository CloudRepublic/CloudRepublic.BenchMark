using CloudRepublic.BenchMark.Application.Models;
using System.Collections.Generic;

namespace CloudRepublic.BenchMark.Orchestrator.Infrastructure
{
    public interface IBenchMarkTypeService
    {
        IEnumerable<BenchMarkType> GetAllTypes();
    }
}
