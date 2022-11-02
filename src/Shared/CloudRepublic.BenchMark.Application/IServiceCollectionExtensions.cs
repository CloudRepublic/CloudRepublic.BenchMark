using System;
using Azure.Core;
using Azure.Data.Tables;
using CloudRepublic.BenchMark.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CloudRepublic.BenchMark.Application;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddBenchMarkData(this IServiceCollection services, Uri endpoint, string tableName, TokenCredential tokenCredential)
    {
        services.AddScoped<IBenchMarkResultRepository>(x => new BenchMarkResultRepository(new TableClient(endpoint, tableName, tokenCredential)));

        return services;
    }
}