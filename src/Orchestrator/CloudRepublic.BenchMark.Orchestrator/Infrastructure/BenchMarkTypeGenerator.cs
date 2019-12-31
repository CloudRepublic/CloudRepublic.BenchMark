using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;
using System;
using System.Collections.Generic;

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
                
                if (provider.ToString() == "Firebase")
                {
                    benchMarkTypes.Add(new BenchMarkType((CloudProvider)provider,
                        HostEnvironment.Linux,
                        Runtime.Nodejs));
                }
                else
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
            }

            return benchMarkTypes;
        }
    }
}