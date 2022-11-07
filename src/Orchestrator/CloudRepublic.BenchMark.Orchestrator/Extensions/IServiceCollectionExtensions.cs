using Microsoft.Extensions.DependencyInjection;
using System;
using CloudRepublic.BenchMark.Application.Statics;

namespace CloudRepublic.BenchMark.Orchestrator.Extensions;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddBenchMarkClients(this IServiceCollection services)
    {
        var benchMarkTypes = BenchMarkTypeGenerator.GetAllTypes();

        foreach (var benchMarkType in benchMarkTypes)
        {
            services.AddHttpClient(benchMarkType.ClientName,
                client =>
                {
                    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable(benchMarkType.UrlName));
                    if (benchMarkType.SetXFunctionsKey)
                    {
                        client.DefaultRequestHeaders.Add("x-functions-key", Environment.GetEnvironmentVariable(benchMarkType.KeyName));
                    }
                });
        }
        return services;
    }
}