namespace CloudRepublic.BenchMark.API.V2.Models;

public record BenchMarkData(string? CloudProvider, string? HostingEnvironment, string? Runtime, string? Language)
{
    public bool ColdPreviousDayPositive { get; set; }
    public double ColdPreviousDayDifference { get; set; }
    public bool WarmPreviousDayPositive { get; set; }
    public double WarmPreviousDayDifference { get; set; }
    public double ColdMedianExecutionTime { get; set; }
    public double WarmMedianExecutionTime { get; set; }
    public List<DataPoint> ColdDataPoints { get; set; } = new();
    public List<DataPoint> WarmDataPoints { get; set; } = new();
}