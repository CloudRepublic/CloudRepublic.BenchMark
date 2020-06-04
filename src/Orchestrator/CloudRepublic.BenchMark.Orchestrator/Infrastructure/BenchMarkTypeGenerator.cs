using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;
using System.Collections.Generic;

namespace CloudRepublic.BenchMark.Orchestrator.Infrastructure
{
    public static class BenchMarkTypeGenerator
    {
        /// <summary>
        /// Creates all the benchmark options to be tested.
        /// foreach type a client is created, functions are called cold/warm, data can be requested.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BenchMarkType> GetAllTypes()
        {

            // zelfde resultaat maar je kan de params "duidelijk" zien en uitbreiden zonder dat de constructor huge wordt
            return new List<BenchMarkType>()
            {
                new BenchMarkType()
                {
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.Csharp,
                     Name = "AzureWindowsCsharp",
                },
                new BenchMarkType()
                {
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.Nodejs,
                     Name = "AzureWindowsNodejs",
                },
                new BenchMarkType()
                {
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.Python,
                     Name = "AzureWindowsPython",
                },
                new BenchMarkType()
                {
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.Java,
                     Name = "AzureWindowsJava",
                },


                new BenchMarkType()
                {
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Csharp,
                     Name = "AzureLinuxCsharp",
                },
                new BenchMarkType()
                {
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Nodejs,
                     Name = "AzureLinuxNodejs",
                },
                new BenchMarkType()
                {
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Python,
                     Name = "AzureLinuxPython",
                },
                new BenchMarkType()
                {
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Java,
                     Name = "AzureLinuxJava",
                },

                new BenchMarkType()
                {
                     CloudProvider = CloudProvider.Firebase,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Nodejs,
                     Name = "FirebaseLinuxNodejs",
                     SetXFunctionsKey = false,
                },


                /// when adding new Benchmark options make sure to follow the following steps:
                /// add a new type into this 'generator'
                /// assign a name, and add the client, url and key to the enviroment.
                /// setup the function
                /// assign a new button in the benchmark.vue tab item array.
            };

            /*// zelfde resultaat maar je kan de enums uitbreiden zonder dat dit ding of de extension crashed/ifjes ingebouwd hoeven te worden
            return new List<BenchMarkType>()
            {
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Windows,Runtime.Csharp,"AzureWindowsCsharpClient"),
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Windows,Runtime.Nodejs),
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Windows,Runtime.Python),

                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Linux,Runtime.Csharp),
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Linux,Runtime.Nodejs),
                new BenchMarkType(CloudProvider.Azure,HostEnvironment.Linux,Runtime.Python),


                new BenchMarkType(CloudProvider.Firebase,HostEnvironment.Linux,Runtime.Nodejs),
            };*/
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