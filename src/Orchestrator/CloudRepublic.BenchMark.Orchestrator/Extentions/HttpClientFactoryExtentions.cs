using System;
using System.Net.Http;
using CloudRepublic.BenchMark.Orchestrator.Domain.Enums;
using CloudRepublic.BenchMark.Orchestrator.Models;

namespace CloudRepublic.BenchMark.Orchestrator.Extentions
{
    public static class HttpClientFactoryExtentions
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(benchMarkType));
            }
        }
    }
}