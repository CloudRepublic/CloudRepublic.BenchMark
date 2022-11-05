using System;
using Azure.Core;
using Azure.Identity;
using CloudRepublic.BenchMark.Application;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Services;
using CloudRepublic.BenchMark.Orchestrator.Extensions;
using CloudRepublic.BenchMark.Orchestrator.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CloudRepublic.BenchMark.Orchestrator.Startup))]
namespace CloudRepublic.BenchMark.Orchestrator;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddAzureAppConfiguration();

        builder.Services.AddBenchMarkData(
            "storage",
            new ChainedTokenCredential(new ManagedIdentityCredential(), new AzureCliCredential()));
            
        builder.Services.AddTransient<IBenchMarkService, BenchMarkService>();
        builder.Services.AddTransient<IBenchMarkTypeService, BenchMarkTypeService>();

        builder.Services.AddBenchMarkClients();
    }
    
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        builder.ConfigurationBuilder.AddEnvironmentVariables();
        
        // Add Azure App Configuration as additional configuration source
        builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
        {
            var configServiceEndpoint = Environment.GetEnvironmentVariable("ConfigurationServiceEndpoint");
            
            var managedIdentityTokenCredential = new ManagedIdentityCredential() as TokenCredential;
            var azureCliCredential = new AzureCliCredential() as TokenCredential;
            var chainedTokenCredential = new ChainedTokenCredential(
                managedIdentityTokenCredential, azureCliCredential);

            options.Connect(new Uri(configServiceEndpoint), chainedTokenCredential);

            options
                .Select("TestFunctions:*")
                .ConfigureRefresh(refreshOptions =>
                {
                    refreshOptions.Register("TestFunctions:Sentinel", refreshAll: true)
                        .SetCacheExpiration(TimeSpan.FromSeconds(30));
                });
        });
    }
}