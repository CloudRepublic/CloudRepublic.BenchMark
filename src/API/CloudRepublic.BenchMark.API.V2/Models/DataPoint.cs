using System.Text.Json.Serialization;

namespace CloudRepublic.BenchMark.API.V2.Models;

public record DataPoint
{
    [JsonPropertyName("createdAt")] 
    public string? CreatedAt { get; set; }
    [JsonPropertyName("executionTime")] 
    public List<int> ExecutionTimes { get; set; } = new();
};