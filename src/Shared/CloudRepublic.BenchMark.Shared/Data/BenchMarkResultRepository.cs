using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Data.Tables;
using CloudRepublic.BenchMark.Data.Entities;
using CloudRepublic.BenchMark.Data.Serializers;
using CloudRepublic.BenchMark.Domain.Entities;
using CloudRepublic.BenchMark.Domain.Enums;

namespace CloudRepublic.BenchMark.Data;

public class BenchMarkResultRepository : IBenchMarkResultRepository
{
    private readonly TableClient _tableClient;

    public BenchMarkResultRepository(TableClient tableClient)
    {
        _tableClient = tableClient;
    }
    
    public async Task AddBenchMarkResultAsync(BenchMarkResult benchMarkResult)
    {
        var entity = new BenchMarkResultEntity
        {
            PartitionKey = BuildPartitionKey(benchMarkResult),
            RowKey = benchMarkResult.Id,
            RecordJson = JsonSerializer.Serialize(benchMarkResult, BenchMarkResultSerializer.Default.BenchMarkResult)
        };
        
        await _tableClient.AddEntityAsync(entity);
    }

    public async Task<IEnumerable<BenchMarkResult>> GetBenchMarkResultsAsync(CloudProvider provider, HostEnvironment environment, Runtime runtime, Language language, string sku, int year, int month, int day)
    {
        var partitionKey = BuildPartitionKey(provider, environment, runtime, language, sku, year, month, day);
        var results = _tableClient.QueryAsync<BenchMarkResultEntity>((x) => x.PartitionKey == partitionKey);

        if (results is null)
        {
            return Array.Empty<BenchMarkResult>();
        }
        
        var benchMarkResults = new List<BenchMarkResult>();
        await foreach (var result in results)
        {
            if (result?.RecordJson is null)
            {
                continue;
            }

            var record = JsonSerializer.Deserialize(result.RecordJson, BenchMarkResultSerializer.Default.BenchMarkResult);
            if (record is null)
            {
                continue;
            }
            
            benchMarkResults.Add(record);
        }

        return benchMarkResults;
    }
    
    private string BuildPartitionKey(BenchMarkResult benchMarkResult)
    {
        return BuildPartitionKey(benchMarkResult.CloudProvider, benchMarkResult.HostingEnvironment, benchMarkResult.Runtime, benchMarkResult.Language, benchMarkResult.Sku, benchMarkResult.CreatedAt.Year, benchMarkResult.CreatedAt.Month, benchMarkResult.CreatedAt.Day);
    }
    
    private string BuildPartitionKey(
        CloudProvider provider, HostEnvironment environment, Runtime runtime, Language language, string sku, int year, int month, int day)
    {
        var partitionKey = $"{year}-{month}-{day}-{provider.GetName()}-{environment.GetName()}-{runtime.GetName()}-{language.GetName()}-{sku}";

        return partitionKey;
    }
}