using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;
using System.Collections.Generic;

namespace CloudRepublic.BenchMark.Orchestrator.Infrastructure
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
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.Fsharp,
                     Name = "AzureWindowsFsharp",
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
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Fsharp,
                     Name = "AzureLinuxFsharp",
                },


                new BenchMarkType()
                {
                     CloudProvider = CloudProvider.Firebase,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.Nodejs,
                     Name = "FirebaseLinuxNodejs",
                     SetXFunctionsKey = false,
                },
            };
        }
    }
}