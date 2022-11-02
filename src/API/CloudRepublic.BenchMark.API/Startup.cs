using System;
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
namespace CloudRepublic.BenchMark.API
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
            
            builder.Services.AddTransient<IBenchMarkResultService, BenchMarkResultService>();
            builder.Services.AddSingleton<IResponseConverterService, ResponseConverterService>();

        }
    }
}