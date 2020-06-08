using CloudRepublic.BenchMark.API.Infrastructure;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Services;
using CloudRepublic.BenchMark.Persistence;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Data.SqlClient;

[assembly: FunctionsStartup(typeof(CloudRepublic.BenchMark.API.Startup))]
namespace CloudRepublic.BenchMark.API
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDbContext<BenchMarkDbContext>(options =>
                options.UseSqlServer(Environment.GetEnvironmentVariable("BenchMarkDatabase")));

            builder.Services.AddTransient<IBenchMarkResultService, BenchMarkResultService>();

            builder.Services.AddSingleton<IResponseConverter, ResponseConverter>();
        }
    }
}