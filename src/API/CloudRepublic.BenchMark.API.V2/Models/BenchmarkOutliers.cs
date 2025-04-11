namespace CloudRepublic.BenchMark.API.V2.Models;

public record BenchmarkOutliers
{
    public required Dictionary<DateTime, int[]> OutliersPerDay;
}