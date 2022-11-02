using System;
using Azure.Identity;
using CloudRepublic.BenchMark.Application;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Services;
using CloudRepublic.BenchMark.Orchestrator.Extensions;
using CloudRepublic.BenchMark.Orchestrator.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CloudRepublic.BenchMark.Orchestrator.Startup))]

namespace CloudRepublic.BenchMark.Orchestrator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            
            var storageSection = configuration.GetSection("storage");
            builder.Services.AddBenchMarkData(
                new Uri(storageSection.GetValue<string>("endpoint")),
                storageSection.GetValue<string>("resultsTableName"),
                new ManagedIdentityCredential());
            
            builder.Services.AddTransient<IBenchMarkService, BenchMarkService>();
            builder.Services.AddTransient<IBenchMarkTypeService, BenchMarkTypeService>();

            builder.Services.AddBenchMarkClients();
        }
    }
}