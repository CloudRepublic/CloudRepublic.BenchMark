using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;
using CloudProvider = CloudRepublic.BenchMark.Domain.Enums.CloudProvider;
using Runtime = CloudRepublic.BenchMark.Domain.Enums.Runtime;
using HostingEnvironment = CloudRepublic.BenchMark.Domain.Enums.HostEnvironment;

namespace CloudRepublic.BenchMark.API.Infrastructure
{
    public class ResponseConverter : IResponseConverter
    {
        public BenchMarkData ConvertToBenchMarkData(IEnumerable<BenchMarkResult> resultDataPoints)
        {
            var benchmarkData = new BenchMarkData();
            
            foreach (var dataPoint in resultDataPoints)
            {
                var parsedCloudProvider = (CloudProvider) Enum.ToObject(typeof(CloudProvider), dataPoint.CloudProvider);
                var parsedHostingEnvironment =
                    (HostingEnvironment) Enum.ToObject(typeof(HostingEnvironment), dataPoint.HostingEnvironment);
                var parsedRuntime = (Runtime) Enum.ToObject(typeof(Runtime), dataPoint.Runtime);

                var cloudProvider =
                    benchmarkData.CloudProviders.SingleOrDefault(c => c.Name == parsedCloudProvider.ToString()) ??
                    new CloudRepublic.BenchMark.API.Models.CloudProvider() {Name = parsedCloudProvider.ToString()};

                var hostingEnvironment =
                    cloudProvider.HostingEnvironments.SingleOrDefault(
                        c => c.Name == parsedHostingEnvironment.ToString()) ??
                    new Models.HostingEnvironment() {Name = parsedHostingEnvironment.ToString()};

                var runtime = hostingEnvironment.Runtimes.SingleOrDefault(c => c.Name == parsedRuntime.ToString()) ??
                              new Models.Runtime() {Name = parsedRuntime.ToString()};

                runtime.DataPoints.Add(new DataPoint()
                    {CreatedAt = dataPoint.CreatedAt, ExecutionTime = dataPoint.RequestDuration});

                var runtimeIndex = hostingEnvironment.Runtimes.FindIndex(c => c.Name == parsedRuntime.ToString());
                if (runtimeIndex > -1)
                {
                    hostingEnvironment.Runtimes[runtimeIndex] = runtime;
                }
                else
                {
                    hostingEnvironment.Runtimes.Add(runtime);
                }

                var hostingEnvironmentIndex =
                    cloudProvider.HostingEnvironments.FindIndex(c => c.Name == parsedHostingEnvironment.ToString());
                if (runtimeIndex > -1)
                {
                    cloudProvider.HostingEnvironments[runtimeIndex] = hostingEnvironment;
                }
                else
                {
                    cloudProvider.HostingEnvironments.Add(hostingEnvironment);
                }

                var cloudProviderIndex =
                    benchmarkData.CloudProviders.FindIndex(c => c.Name == parsedCloudProvider.ToString());
                if (cloudProviderIndex > -1)
                {
                    benchmarkData.CloudProviders[cloudProviderIndex] = cloudProvider;
                }
                else
                {
                    benchmarkData.CloudProviders.Add(cloudProvider);
                }
            }

            return benchmarkData;
        }
    }
}