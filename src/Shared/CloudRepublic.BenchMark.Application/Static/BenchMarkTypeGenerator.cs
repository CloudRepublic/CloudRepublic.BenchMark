using CloudRepublic.BenchMark.Application.Models;
using CloudRepublic.BenchMark.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CloudRepublic.BenchMark.Application.Statics;

public static class BenchMarkTypeGenerator
{
     /// <summary>
     /// Creates all the benchmark options to be tested.
     /// foreach type a client is created, functions are called cold/warm.
     /// </summary>
     /// <returns></returns>
     public static IEnumerable<BenchMarkType> GetAllTypesFromConfiguration(this IConfiguration configuration)
     {
          var sections = configuration.GetChildren();
          return sections.Where(s => s.Key != "Sentinel").Select(s => new BenchMarkType
          {
               Title = s["Title"],
               Name = s.Key,
               Language = new EnumFromString<Language>(s["Language"]).Value,
               Runtime = new EnumFromString<Runtime>(s["Runtime"]).Value,
               CloudProvider = new EnumFromString<CloudProvider>(s["CloudProvider"]).Value,
               HostEnvironment = new EnumFromString<HostEnvironment>(s["HostEnvironment"]).Value,
               Sku = s["Sku"],
               TestEndpoint = s["TestEndpoint"],
               AuthenticationHeaderName = s["AuthenticationHeaderName"],
               AuthenticationHeaderValue = s["AuthenticationHeaderValue"],
          });
     }
}