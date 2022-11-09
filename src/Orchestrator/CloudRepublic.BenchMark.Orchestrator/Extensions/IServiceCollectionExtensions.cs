using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Application.Statics;
using Microsoft.Extensions.Configuration;

namespace CloudRepublic.BenchMark.Orchestrator.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddBenchMark(this IServiceCollection services)
    {
        services.AddScoped((s) =>
        {
            var configuration = s.GetRequiredService<IConfiguration>();
            var configurationSection = configuration.GetSection("BenchMarkTests");
            return configurationSection.GetAllTypesFromConfiguration();
        });

        return services;
    }
}