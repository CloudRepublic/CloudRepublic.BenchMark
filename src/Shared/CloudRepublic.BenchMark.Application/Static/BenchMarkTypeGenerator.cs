using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;
using System.Collections.Generic;

namespace CloudRepublic.BenchMark.Application.Statics
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
             return new List<BenchMarkType>
            {
                new()
                {
                     Name = "AzureWindowsCsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.FunctionsV4,
                     Language = Language.Csharp
                },
                new()
                {
                     Name = "AzureWindowsNodejs",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.FunctionsV4,
                     Language = Language.Nodejs
                },
                new()
                {
                     Name = "AzureWindowsJava",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.FunctionsV4,
                     Language = Language.Java
                },
                new()
                {
                     Name = "AzureWindowsFsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Windows,
                     Runtime = Runtime.FunctionsV4,
                     Language = Language.Fsharp
                },
                new()
                {
                     Name = "AzureLinuxCsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.FunctionsV4,
                     Language = Language.Csharp
                },
                new()
                {
                     Name = "AzureLinuxNodejs",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.FunctionsV4,
                     Language = Language.Nodejs
                },
                new()
                {
                     Name = "AzureLinuxPython",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.FunctionsV4,
                     Language = Language.Python
                },
                new()
                {
                     Name = "AzureLinuxJava",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.FunctionsV4,
                     Language = Language.Java
                },
                new()
                {
                     Name = "AzureLinuxFsharp",
                     CloudProvider = CloudProvider.Azure,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.FunctionsV4,
                     Language = Language.Fsharp
                },

                new()
                {
                     Name = "FirebaseLinuxNodejs",
                     CloudProvider = CloudProvider.Firebase,
                     HostEnvironment = HostEnvironment.Linux,
                     Runtime = Runtime.FunctionsV4,
                     Language = Language.Nodejs,
                     SetXFunctionsKey = false,
                },
            };
        }
    }
}