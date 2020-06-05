using System;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Services;
using CloudRepublic.BenchMark.Orchestrator.Extensions;
using CloudRepublic.BenchMark.Orchestrator.Infrastructure;
using CloudRepublic.BenchMark.Persistence;
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

            builder.Services.AddTransient<IBenchMarkService, BenchMarkService>();
            builder.Services.AddTransient<IBenchMarkTypeService, BenchMarkTypeService>();

            builder.Services.AddBenchMarkClients();
        }
    }
}