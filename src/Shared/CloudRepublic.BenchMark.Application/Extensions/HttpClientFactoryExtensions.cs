using System;
using System.Net.Http;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;

namespace CloudRepublic.BenchMark.Application.Extensions
{
    public static class HttpClientFactoryExtensions
    {
        public static HttpClient CreateBenchMarkClient(this IHttpClientFactory httpClientFactory,
            BenchMarkType benchMarkType)
        {
            switch (benchMarkType.CloudProvider)
            {
                case CloudProvider.Azure when benchMarkType.HostEnvironment == HostEnvironment.Windows &&
                                              benchMarkType.Runtime == Runtime.Csharp:
                    return httpClientFactory.CreateClient("AzureWindowsCsharpClient");
                case CloudProvider.Azure when benchMarkType.HostEnvironment == HostEnvironment.Linux &&
                                              benchMarkType.Runtime == Runtime.Csharp:
                    return httpClientFactory.CreateClient("AzureLinuxCsharpClient");
                case CloudProvider.Azure when benchMarkType.HostEnvironment == HostEnvironment.Windows &&
                                              benchMarkType.Runtime == Runtime.Nodejs:
                    return httpClientFactory.CreateClient("AzureWindowsNodejsClient");
                case CloudProvider.Azure when benchMarkType.HostEnvironment == HostEnvironment.Linux &&
                                              benchMarkType.Runtime == Runtime.Nodejs:
                    return httpClientFactory.CreateClient("AzureLinuxNodejsClient");
                case CloudProvider.Azure when benchMarkType.HostEnvironment == HostEnvironment.Windows &&
                                              benchMarkType.Runtime == Runtime.Fsharp:
                    return httpClientFactory.CreateClient("AzureWindowsFsharpClient");
                case CloudProvider.Azure when benchMarkType.HostEnvironment == HostEnvironment.Linux &&
                                              benchMarkType.Runtime == Runtime.Fsharp:
                    return httpClientFactory.CreateClient("AzureLinuxFsharpClient");
                case CloudProvider.Firebase when benchMarkType.HostEnvironment == HostEnvironment.Linux &&
                                              benchMarkType.Runtime == Runtime.Nodejs:
                    return httpClientFactory.CreateClient("FirebaseLinuxNodejsClient");
                default:
                    throw new ArgumentOutOfRangeException(nameof(benchMarkType));
            }
        }
    }

    
}