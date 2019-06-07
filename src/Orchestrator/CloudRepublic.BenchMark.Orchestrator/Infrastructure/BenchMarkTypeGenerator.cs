using System;
using System.Collections.Generic;
using CloudRepublic.BenchMark.Orchestrator.Application.Models;
using CloudRepublic.BenchMark.Orchestrator.Domain.Enums;

namespace CloudRepublic.BenchMark.Orchestrator.Infrastructure
{
    public static class BenchMarkTypeGenerator
    {
        public static IEnumerable<BenchMarkType> Generate()
        {
            var cloudProviders = Enum.GetValues(typeof(CloudProvider));
            var hostingEnvironments = Enum.GetValues(typeof(HostEnvironment));
            var runtimes = Enum.GetValues(typeof(Runtime));

            var benchMarkTypes = new List<BenchMarkType>();

            foreach (var provider in cloudProviders)
            {
                foreach (var hostingEnvironment in hostingEnvironments)
                {
                    foreach (var runtime in runtimes)
                    {
                        benchMarkTypes.Add(new BenchMarkType((CloudProvider)provider,
                            (HostEnvironment)hostingEnvironment,
                            (Runtime)runtime));
                    }
                }
            }

            return benchMarkTypes;
        }
    }
}