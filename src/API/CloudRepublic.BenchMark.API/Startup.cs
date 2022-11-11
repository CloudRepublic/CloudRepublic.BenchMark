using System;
using Azure.Core;
using Azure.Identity;
using CloudRepublic.BenchMark.API.Interfaces;
using CloudRepublic.BenchMark.API.Services;
using CloudRepublic.BenchMark.Application;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CloudRepublic.BenchMark.API.Startup))]
namespace CloudRepublic.BenchMark.API;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddBenchMarkData(
            "storage",
            new ChainedTokenCredential(new ManagedIdentityCredential(), new AzureCliCredential()));

        builder.Services.AddTransient<IBenchMarkResultService, BenchMarkResultService>();
        builder.Services.AddSingleton<IResponseConverterService, ResponseConverterService>();
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        builder.ConfigurationBuilder.AddEnvironmentVariables();

        // // Add Azure App Configuration as additional configuration source
        // builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
        // {
        //     var configServiceEndpoint = Environment.GetEnvironmentVariable("ConfigurationServiceEndpoint");
        //
        //     var managedIdentityTokenCredential = new ManagedIdentityCredential() as TokenCredential;
        //     var azureCliCredential = new AzureCliCredential() as TokenCredential;
        //     var chainedTokenCredential = new ChainedTokenCredential(
        //         managedIdentityTokenCredential, azureCliCredential);
        //
        //     options.Connect(new Uri(configServiceEndpoint), chainedTokenCredential);
        //
        //     options
        //         .Select("BenchMarkTests:*")
        //         .ConfigureRefresh(refreshOptions =>
        //         {
        //             refreshOptions.Register("BenchMarkTests:Sentinel", refreshAll: true)
        //                 .SetCacheExpiration(TimeSpan.FromSeconds(30));
        //         });
        // });
    }
}