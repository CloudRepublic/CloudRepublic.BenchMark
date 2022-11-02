using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Data.Tables;
using CloudRepublic.BenchMark.Data.Entities;
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
            RecordJson = JsonSerializer.Serialize(benchMarkResult)
        };
        
        await _tableClient.AddEntityAsync(entity);
    }

    public async IAsyncEnumerable<BenchMarkResult> GetBenchMarkResultsAsync(CloudProvider provider, HostEnvironment environment, Runtime runtime, Language language, int year, int month)
    {
        var partitionKey = BuildPartitionKey(provider, environment, runtime, language, year, month);
        var results = _tableClient.QueryAsync<BenchMarkResultEntity>((x) => x.PartitionKey == partitionKey);

        if (results is null)
        {
            yield break;
        }
        
        await foreach (var result in results)
        {
            if (result is null)
            {
                continue;
            }
            
            yield return JsonSerializer.Deserialize<BenchMarkResult>(result.RecordJson);
        }
    }
    
    private string BuildPartitionKey(BenchMarkResult benchMarkResult)
    {
        return BuildPartitionKey(benchMarkResult.CloudProvider, benchMarkResult.HostingEnvironment, benchMarkResult.Runtime, benchMarkResult.Language, benchMarkResult.CreatedAt.Year, benchMarkResult.CreatedAt.Month);
    }
    
    private string BuildPartitionKey(
        CloudProvider provider, HostEnvironment environment, Runtime runtime, Language language, int year, int month)
    {
        var partitionKey = $"{year}-{month}-{provider.GetName()}-{environment.GetName()}-{runtime.GetName()}-{language.GetName()}";

        return partitionKey;
    }
}