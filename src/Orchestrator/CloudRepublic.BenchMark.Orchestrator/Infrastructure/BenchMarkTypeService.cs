using CloudRepublic.BenchMark.Application.Models;
using System.Collections.Generic;

namespace CloudRepublic.BenchMark.Orchestrator.Infrastructure
{
    public class BenchMarkTypeService : IBenchMarkTypeService
    {
        public IEnumerable<BenchMarkType> GetAllTypes()
        {
            return BenchMarkTypeGenerator.GetAllTypes();
        }
    }
}
