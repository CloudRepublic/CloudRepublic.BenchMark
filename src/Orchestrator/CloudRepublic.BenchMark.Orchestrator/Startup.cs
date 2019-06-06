using System;
using CloudRepublic.BenchMark.Orchestrator.Application.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.Persistence;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CloudRepublic.BenchMark.Orchestrator.Startup))]

namespace CloudRepublic.BenchMark.Orchestrator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<IBenchMarkDbContext, BenchMarkDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("BenchMarkDatabase")));


            builder.Services.AddHttpClient("AzureWindowsCsharpClient",
                client =>
                {
                    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("AzureWindowsCsharpUrl"));
                });
        }
    }
}