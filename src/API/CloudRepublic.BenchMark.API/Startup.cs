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
        #if DEBUG
        builder.Services.AddBenchMarkData("storage", new AzureCliCredential());
        #else
        builder.Services.AddBenchMarkData("storage", new ManagedIdentityCredential());
        #endif

        builder.Services.AddTransient<IBenchMarkResultService, BenchMarkResultService>();
        builder.Services.AddSingleton<IResponseConverterService, ResponseConverterService>();
    }

    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        builder.ConfigurationBuilder.AddEnvironmentVariables();
        
        // Add Azure App Configuration as additional configuration source
        builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
        {
            var configServiceEndpoint = Environment.GetEnvironmentVariable("ConfigurationServiceEndpoint");
            
#if DEBUG
            if (configServiceEndpoint is { })
            {
                options.Connect(new Uri(configServiceEndpoint), new AzureCliCredential());
            }
            else
            {
                var connectionString = Environment.GetEnvironmentVariable("ConfigurationServiceConnectionString");
                options.Connect(connectionString);
            }
#else
            options.Connect(new Uri(configServiceEndpoint), new ManagedIdentityCredential());
#endif
        
            options
                .Select("BenchMarkTests:*")
                .ConfigureRefresh(refreshOptions =>
                {
                    refreshOptions.Register("BenchMarkTests:Sentinel", refreshAll: true)
                        .SetCacheExpiration(TimeSpan.FromSeconds(30));
                });
        });
    }
}