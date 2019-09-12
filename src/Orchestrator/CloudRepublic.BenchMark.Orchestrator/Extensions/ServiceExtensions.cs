using System;
using Microsoft.Extensions.DependencyInjection;

namespace CloudRepublic.BenchMark.Orchestrator.Extensions
{
    public static class ServiceExtensions
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
                    client.DefaultRequestHeaders.Add("x-functions-key", Environment.GetEnvironmentVariable("AzureLinuxCsharpKey"));
                });            
            
            services.AddHttpClient("AzureWindowsNodejsClient",
                client =>
                {
                    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("AzureWindowsNodejsUrl"));
                    client.DefaultRequestHeaders.Add("x-functions-key", Environment.GetEnvironmentVariable("AzureWindowsNodejsKey"));
                });
            
            services.AddHttpClient("AzureLinuxNodejsClient",
                client =>
                {
                    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("AzureLinuxNodejsUrl"));
                    client.DefaultRequestHeaders.Add("x-functions-key", Environment.GetEnvironmentVariable("AzureLinuxNodejsKey"));
                });

            services.AddHttpClient("FirebaseLinuxNodejsClient",
                client =>
                {
                    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("FirebaseLinuxNodejsUrl"));
                });

            return services;
        }
    }
}