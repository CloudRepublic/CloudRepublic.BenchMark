using System;
using Azure.Core;
using Azure.Data.Tables;
using CloudRepublic.BenchMark.Application.Statics;
using CloudRepublic.BenchMark.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudRepublic.BenchMark.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBenchMarkData(this IServiceCollection services, string sectionKey, TokenCredential tokenCredential)
    {
        services.AddScoped(s =>
        {
            var configuration = s.GetRequiredService<IConfiguration>();
            var configurationSection = configuration.GetSection("BenchMarkTests");
            return configurationSection.GetAllTypesFromConfiguration();
        });
        
        services.AddScoped<IBenchMarkResultRepository>(s =>
        {
            var configuration = s.GetRequiredService<IConfiguration>();
            var storageSection = configuration.GetSection(sectionKey);
            
            return new BenchMarkResultRepository(new TableClient(
                new Uri(storageSection["tableEndpoint"]),
                storageSection["resultsTableName"],
                tokenCredential)
            );
        });

        return services;
    }
}