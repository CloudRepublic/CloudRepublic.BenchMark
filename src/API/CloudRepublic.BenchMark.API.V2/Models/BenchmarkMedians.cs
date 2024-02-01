using System.Text.Json.Serialization;

namespace CloudRepublic.BenchMark.API.V2.Models;

public record BenchmarkMedians
{
    [JsonPropertyName("currentDay")]
    public double CurrentDay;
    
    [JsonPropertyName("previousDay")]
    public double PreviousDay;
    
    [JsonPropertyName("differencePercentage")]
    public double DifferencePercentage => Math.Abs(CurrentDay) == 0 ? 0 : Math.Round((CurrentDay - PreviousDay) / Math.Abs(CurrentDay) * 100, 2);
}