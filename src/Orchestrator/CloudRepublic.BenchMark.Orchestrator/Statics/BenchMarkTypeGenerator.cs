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
                // WINDOWS OPERATING SYSTEMS
                new BenchMarkType()
                {
                     Name = "AzureWindowsCsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Language = Language.Csharp,
                },
                new BenchMarkType()
                {
                     Name = "AzureWindowsNodejs",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Language = Language.Nodejs,
                },
                new BenchMarkType()
                {
                     Name = "AzureWindowsPython",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Language = Language.Python,
                },
                new BenchMarkType()
                {
                     Name = "AzureWindowsJava",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Language = Language.Java,
                },
                new BenchMarkType()
                {
                     Name = "AzureWindowsFsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Language = Language.Fsharp,
                },

                // LINUX OPERATING SYSTEMS
                new BenchMarkType()
                {
                     Name = "AzureLinuxCsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Language = Language.Csharp,
                },
                new BenchMarkType()
                {
                     Name = "AzureLinuxNodejs",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Language = Language.Nodejs,
                },
                new BenchMarkType()
                {
                     Name = "AzureLinuxPython",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Language = Language.Python,
                },
                new BenchMarkType()
                {
                     Name = "AzureLinuxJava",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Language = Language.Java,
                },
                new BenchMarkType()
                {
                     Name = "AzureLinuxFsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Language = Language.Fsharp,
                },

                // AZURE FUNCTIONS V3
                new BenchMarkType()
                {
                     Name = "AzureWindowsCsharpV3",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Language = Language.Csharp,
                     AzureRuntimeVersion = AzureRuntimeVersion.Version_3,
                },

                new BenchMarkType()
                {
                     Name = "AzureWindowsFsharpV3",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Language = Language.Fsharp,
                     AzureRuntimeVersion = AzureRuntimeVersion.Version_3,
                },
                new BenchMarkType()
                {
                     Name = "AzureWindowsNodejsV3",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Language = Language.Nodejs,
                     AzureRuntimeVersion = AzureRuntimeVersion.Version_3,
                },
                new BenchMarkType()
                {
                     Name = "AzureWindowsJavaV3",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Language = Language.Java,
                     AzureRuntimeVersion = AzureRuntimeVersion.Version_3,
                },

                // FIREBASE
                new BenchMarkType()
                {
                     Name = "FirebaseLinuxNodejs",
                     CloudProvider = CloudProvider.Firebase,
                     HostEnvironment = HostEnvironment.Linux,
                     Language = Language.Nodejs,
                     SetXFunctionsKey = false,
                     AzureRuntimeVersion = AzureRuntimeVersion.Not_Azure,
                },
            };
        }
    }
}