using Azure.Identity;
using CloudRepublic.BenchMark.API.V2.Interfaces;
using CloudRepublic.BenchMark.API.V2.Services;
using CloudRepublic.BenchMark.Application;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
#if DEBUG
        services.AddBenchMarkData("storage", new AzureCliCredential());
#else
        services.AddBenchMarkData("storage", new ManagedIdentityCredential());
#endif
        services.AddTransient<IBenchMarkResultService, BenchMarkResultService>();
        services.AddSingleton<IResponseConverterService, ResponseConverterService>();
        
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddEnvironmentVariables();
        
        // Add Azure App Configuration as additional configuration source
        builder.AddAzureAppConfiguration(options =>
        {
            var configServiceEndpoint = Environment.GetEnvironmentVariable("ConfigurationServiceEndpoint");
            if (configServiceEndpoint is null)
            {
                throw new Exception("ConfigurationServiceEndpoint is not set");
            }
            
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
                        .SetCacheExpiration(TimeSpan.FromSeconds(200));
                });
        });
    })
    .Build();

host.Run();