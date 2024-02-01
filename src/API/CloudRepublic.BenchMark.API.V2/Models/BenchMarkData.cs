using System.Text.Json.Serialization;

namespace CloudRepublic.BenchMark.API.V2.Models;

public record BenchMarkData(string? CloudProvider, string? HostingEnvironment, string? Runtime, string? Language)
{
    [JsonPropertyName("coldPreviousDayPositive")]
    public bool ColdPreviousDayPositive { get; set; }
    
    [JsonPropertyName("coldPreviousDayDifference")]
    public double ColdPreviousDayDifference { get; set; }
    
    [JsonPropertyName("warmPreviousDayPositive")]
    public bool WarmPreviousDayPositive { get; set; }
    
    [JsonPropertyName("warmPreviousDayDifference")]
    public double WarmPreviousDayDifference { get; set; }
    
    [JsonPropertyName("coldMedianExecutionTime")]
    public double ColdMedianExecutionTime { get; set; }
    
    [JsonPropertyName("warmMedianExecutionTime")]
    public double WarmMedianExecutionTime { get; set; }
    
    [JsonPropertyName("coldAverageExecutionTime")]
    public List<DataPoint> ColdDataPoints { get; set; } = new();
    
    [JsonPropertyName("warmAverageExecutionTime")]
    public List<DataPoint> WarmDataPoints { get; set; } = new();
}