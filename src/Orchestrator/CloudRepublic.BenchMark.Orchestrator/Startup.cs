using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Services;
using CloudRepublic.BenchMark.Data;
using CloudRepublic.BenchMark.Orchestrator.Extensions;
using CloudRepublic.BenchMark.Orchestrator.Interfaces;
using CloudRepublic.BenchMark.Orchestrator.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(CloudRepublic.BenchMark.Orchestrator.Startup))]

namespace CloudRepublic.BenchMark.Orchestrator
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<BenchMarkDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("BenchMarkDatabase")));

            builder.Services.AddTransient<IBenchMarkService, BenchMarkService>();

            builder.Services.AddTransient<IBenchMarkTypeService, BenchMarkTypeService>();

            builder.Services.AddBenchMarkClients();
        }
    }
}