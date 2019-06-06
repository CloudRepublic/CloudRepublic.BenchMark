using System;
using Microsoft.Extensions.DependencyInjection;

namespace CloudRepublic.BenchMark.Orchestrator.Extentions
{
    public static class ServiceExtentions
    {
        public static IServiceCollection AddBenchMarkClients(this IServiceCollection services)
        {
            services.AddHttpClient("AzureWindowsCsharpClient",
                client =>
                {
                    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("AzureWindowsCsharpUrl"));
                    client.DefaultRequestHeaders.Add("x-functions-key",
                        Environment.GetEnvironmentVariable("AzureWindowsCsharpKey"));
                });

            services.AddHttpClient("AzureLinuxCsharpClient",
                client =>
                {
                    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("AzureLinuxCsharpUrl"));
                    client.DefaultRequestHeaders.Add("x-functions-key", "AzureLinuxCsharpKey");
                });

            return services;
        }
    }
}