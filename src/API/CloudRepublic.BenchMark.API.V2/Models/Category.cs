using System.Text.Json.Serialization;
using CloudRepublic.BenchMark.Domain.Enums;

namespace CloudRepublic.BenchMark.API.V2.Models;

public record Category
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }
    
    [JsonPropertyName("cloud")]
    [JsonConverter(typeof(JsonStringEnumConverter<CloudProvider>))]
    public CloudProvider Cloud { get; set; }
    
    [JsonPropertyName("os")]
    [JsonConverter(typeof(JsonStringEnumConverter<HostEnvironment>))]
    public HostEnvironment Os { get; set; }
    
    [JsonPropertyName("runtime")]
    [JsonConverter(typeof(JsonStringEnumConverter<Runtime>))]
    public Runtime Runtime { get; set; }
    
    [JsonPropertyName("language")]
    [JsonConverter(typeof(JsonStringEnumConverter<Language>))]
    public Language Language { get; set; }
    
    [JsonPropertyName("sku")]
    public string? Sku { get; set; }
}