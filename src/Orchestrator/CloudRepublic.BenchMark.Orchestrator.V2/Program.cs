using Azure.Core;
using Azure.Identity;
using CloudRepublic.BenchMark.Application;
using CloudRepublic.BenchMark.Orchestrator.V2.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.V2.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => {
        services.AddAzureAppConfiguration();

        services.AddBenchMarkData(
            "storage",
            new ChainedTokenCredential(new ManagedIdentityCredential(), new AzureCliCredential()));
            
        services.AddTransient<IBenchMarkService, BenchMarkService>();
        services.AddTransient<IBenchMarkTypeService, BenchMarkTypeService>();
        
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        
        services.AddHttpClient("benchmarkTester");
    })
    .ConfigureAppConfiguration(builder => {
            builder.AddEnvironmentVariables();
        
            // Add Azure App Configuration as additional configuration source
            builder.AddAzureAppConfiguration(options =>
            {
                var configServiceEndpoint = Environment.GetEnvironmentVariable("ConfigurationServiceEndpoint");
                if (configServiceEndpoint is null)
                {
                    throw new Exception("ConfigurationServiceEndpoint is not set");
                }
            
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
                            .SetRefreshInterval(TimeSpan.FromSeconds(30));
                    });
            });
        })
        .Build();

host.Run();