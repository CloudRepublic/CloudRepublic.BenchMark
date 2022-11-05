using System;
using Azure.Core;
using Azure.Data.Tables;
using CloudRepublic.BenchMark.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudRepublic.BenchMark.Application;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddBenchMarkData(this IServiceCollection services, string sectionKey, TokenCredential tokenCredential)
    {
        services.AddScoped<IBenchMarkResultRepository>(s =>
        {
            var configuration = s.GetRequiredService<IConfiguration>();
            var storageSection = configuration.GetSection(sectionKey);
            
            return new BenchMarkResultRepository(new TableClient(
                new Uri(storageSection["endpoint"]),
                storageSection["resultsTableName"],
                tokenCredential)
            );
        });

        return services;
    }
}