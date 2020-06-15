using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;
using System.Collections.Generic;

namespace CloudRepublic.BenchMark.Orchestrator.Statics
{
    public static class BenchMarkTypeGenerator
    {
        /// <summary>
        /// Creates all the benchmark options to be tested.
        /// foreach type a client is created, functions are called cold/warm.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BenchMarkType> GetAllTypes()
        {

            // zelfde resultaat maar je kan de params "duidelijk" zien en uitbreiden zonder dat de constructor huge wordt
            return new List<BenchMarkType>()
            {
                new BenchMarkType()
                {
                     Name = "AzureWindowsCsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.Csharp,
                },
                new BenchMarkType()
                {
                     Name = "AzureWindowsNodejs",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.Nodejs,
                },
                new BenchMarkType()
                {
                     Name = "AzureWindowsJava",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.Java,
                },
                new BenchMarkType()
                {
                     Name = "AzureWindowsFsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.Fsharp,
                },


                new BenchMarkType()
                {
                     Name = "AzureLinuxCsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Csharp,
                },
                new BenchMarkType()
                {
                     Name = "AzureLinuxNodejs",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Nodejs,
                },
                new BenchMarkType()
                {
                     Name = "AzureLinuxPython",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Python,
                },
                new BenchMarkType()
                {
                     Name = "AzureLinuxJava",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Java,
                },
                new BenchMarkType()
                {
                     Name = "AzureLinuxFsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Fsharp,
                },


                new BenchMarkType()
                {
                     Name = "FirebaseLinuxNodejs",
                     CloudProvider = CloudProvider.Firebase,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Nodejs,
                     SetXFunctionsKey = false,
                },
            };
        }
    }
}