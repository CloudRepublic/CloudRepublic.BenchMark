using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using CloudRepublic.BenchMark.API.V2.Interfaces;
using CloudRepublic.BenchMark.API.V2.Models;
using CloudRepublic.BenchMark.API.V2.Serializers;
using CloudRepublic.BenchMark.Domain.Enums;
using CloudRepublic.BenchMark.Data;

namespace CloudRepublic.BenchMark.API.V2.Services;

public class ResponseCacheService(BlobContainerClient blobContainerClient) : IResponseCacheService
{
    public async Task StoreBenchMarkResultAsync(BenchMarkData result)
    {
        var blobName = CreateBlobName(result);
        var blobClient = blobContainerClient.GetBlobClient(blobName);
        
        var json = JsonSerializer.Serialize(result, StatisticsSerializerContext.Default.BenchMarkData);
        var bytes = Encoding.UTF8.GetBytes(json);
        
        await blobClient.UploadAsync(new MemoryStream(bytes), true);
    }

    public async Task<string> RunBenchMarksAsync(CloudProvider cloudProvider, HostEnvironment hostingEnvironment, Runtime runtime, Language language, string sku)
    {
        var blobName = CreateBlobName(cloudProvider, hostingEnvironment, runtime, language, sku);
        var blobClient = blobContainerClient.GetBlobClient(blobName);
        var memoryStream = new MemoryStream();
        await blobClient.DownloadToAsync(memoryStream);
        
        memoryStream.Position = 0;
        using var reader = new StreamReader(memoryStream);
        return await reader.ReadToEndAsync();
    }
    
    private string CreateBlobName(BenchMarkData result)
    {
        return CreateBlobName(result.CloudProvider, result.HostingEnvironment, result.Runtime, result.Language,
            result.Sku);
    }
    
    private string CreateBlobName(CloudProvider cloudProvider, HostEnvironment hostingEnvironment, Runtime runtime, Language language, string sku)
    {
        return CreateBlobName(cloudProvider.GetName(), hostingEnvironment.GetName(), runtime.GetName(), language.GetName(),
            sku);
    }
    
    private string CreateBlobName(string cloudProvider, string hostingEnvironment, string runtime, string language, string sku)
    {
        return $"{cloudProvider}_{hostingEnvironment}_{runtime}_{language}_{sku}.json";
    }
}