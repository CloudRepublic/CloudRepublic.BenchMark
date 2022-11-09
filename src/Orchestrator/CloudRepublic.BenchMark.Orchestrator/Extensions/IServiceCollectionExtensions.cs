using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Application.Statics;
using Microsoft.Extensions.Configuration;

namespace CloudRepublic.BenchMark.Orchestrator.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddBenchMark(this IServiceCollection services, IConfiguration configurationSection)
    {
        var benchMarkTypes = configurationSection.GetAllTypesFromConfiguration();
        
        services.AddSingleton(benchMarkTypes);
        foreach (var benchMarkType in benchMarkTypes)
        {
            services.AddHttpClient(benchMarkType.Name);
        }
        return services;
    }
}