namespace CloudRepublic.BenchMark.API.V2.Models;

public record DataPoint
{
    public string? CreatedAt { get; set; }
    public List<int> ExecutionTimes { get; set; } = new();
};