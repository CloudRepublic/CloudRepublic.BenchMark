using Azure.Core;
using Azure.Identity;
using CloudRepublic.BenchMark.Application;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Services;
using CloudRepublic.BenchMark.Orchestrator.V2.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.V2.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddAzureAppConfiguration();

        services.AddBenchMarkData(
            "storage",
            new ChainedTokenCredential(new ManagedIdentityCredential(), new AzureCliCredential()));
            
        services.AddTransient<IBenchMarkService, BenchMarkService>();
        services.AddTransient<IBenchMarkTypeService, BenchMarkTypeService>();
        
        services.AddHttpClient("benchmarkTester");
    })
    .ConfigureAppConfiguration(builder =>
        {
            builder.AddEnvironmentVariables();

            // Add Azure App Configuration as additional configuration source
            builder.AddAzureAppConfiguration(options =>
            {
                builder.AddEnvironmentVariables();

                // Add Azure App Configuration as additional configuration source
                builder.AddAzureAppConfiguration(options =>
                {
                    var configServiceEndpoint = Environment.GetEnvironmentVariable("ConfigurationServiceEndpoint");

                    var managedIdentityTokenCredential = new ManagedIdentityCredential() as TokenCredential;
                    var azureCliCredential = new AzureCliCredential() as TokenCredential;
                    var chainedTokenCredential = new ChainedTokenCredential(
                        managedIdentityTokenCredential, azureCliCredential);

                    options.Connect(new Uri(configServiceEndpoint), chainedTokenCredential);

                    options
                        .Select("BenchMarkTests:*")
                        .ConfigureRefresh(refreshOptions =>
                        {
                            refreshOptions.Register("BenchMarkTests:Sentinel", refreshAll: true)
                                .SetCacheExpiration(TimeSpan.FromSeconds(30));
                        });
                });
            });
        })
        .Build();

host.Run();