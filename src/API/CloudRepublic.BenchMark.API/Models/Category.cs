using CloudRepublic.BenchMark.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CloudRepublic.BenchMark.API.Models;

public class Category
{
    [JsonProperty("title")]
    public string Title { get; set; }
    
    [JsonProperty("cloud")]
    [JsonConverter(typeof(StringEnumConverter))]
    public CloudProvider Cloud { get; set; }
    
    [JsonProperty("os")]
    [JsonConverter(typeof(StringEnumConverter))]
    public HostEnvironment Os { get; set; }
    
    [JsonProperty("runtime")]
    [JsonConverter(typeof(StringEnumConverter))]
    public Runtime Runtime { get; set; }
    
    [JsonProperty("language")]
    [JsonConverter(typeof(StringEnumConverter))]
    public Language Language { get; set; }
    
    [JsonProperty("sku")]
    public string Sku { get; set; }
}