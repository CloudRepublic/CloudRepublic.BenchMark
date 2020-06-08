using CloudRepublic.BenchMark.API.Interfaces;
using CloudRepublic.BenchMark.API.Services;
using CloudRepublic.BenchMark.Application.Interfaces;
using CloudRepublic.BenchMark.Application.Services;
using CloudRepublic.BenchMark.Persistence;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            builder.Services.AddSingleton<IResponseConverterService, ResponseConverterService>();
        }
    }
}