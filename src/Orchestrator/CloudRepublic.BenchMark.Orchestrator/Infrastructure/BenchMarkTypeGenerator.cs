using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;
using System.Collections.Generic;

namespace CloudRepublic.BenchMark.Orchestrator.Infrastructure
{
    public static class BenchMarkTypeGenerator
    {
        public static BenchMarkType FireBaseNodeJs = new BenchMarkType(CloudProvider.Firebase, HostEnvironment.Linux, Runtime.Nodejs);
        public static IEnumerable<BenchMarkType> Generate()
        {

            // zelfde resultaat maar je kan de params duidelijk zien en uitbreiden zonder dat de constructor huge wordt
            return new List<BenchMarkType>()
            {
                new BenchMarkType()
                {
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.Csharp,
                     ClientName = "AzureWindowsCsharpClient",
                },
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Windows,Runtime.Nodejs),
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Windows,Runtime.Python),

                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Linux,Runtime.Csharp),
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Linux,Runtime.Nodejs),
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Linux,Runtime.Python),


                new BenchMarkType(CloudProvider.Firebase,HostEnvironment.Linux,Runtime.Nodejs),
            };

            // zelfde resultaat maar je kan de enums uitbreiden zonder dat dit ding crashed/ifjes ingebouwd hoeven te worden
            return new List<BenchMarkType>()
            {
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Windows,Runtime.Csharp),
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Windows,Runtime.Nodejs),
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Windows,Runtime.Python),

                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Linux,Runtime.Csharp),
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Linux,Runtime.Nodejs),
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Linux,Runtime.Python),


                new BenchMarkType(CloudProvider.Firebase,HostEnvironment.Linux,Runtime.Nodejs),
            };
            /*
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

                        return benchMarkTypes;*/
        }
    }
}