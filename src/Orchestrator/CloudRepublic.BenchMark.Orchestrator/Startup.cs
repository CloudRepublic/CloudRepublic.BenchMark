using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CloudRepublic.BenchMark.Orchestrator.Startup))]

namespace CloudRepublic.BenchMark.Orchestrator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient("AzureWindowsCsharpClient",
                client => { client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("AzureWindowsCsharpUrl")); });
        }
    }
}