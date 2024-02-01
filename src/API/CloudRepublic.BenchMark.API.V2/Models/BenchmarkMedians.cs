namespace CloudRepublic.BenchMark.API.V2.Models;

public record BenchmarkMedians
{
    public double CurrentDay;
    public double PreviousDay;
    public double DifferencePercentage => Math.Abs(CurrentDay) == 0 ? 0 : Math.Round((CurrentDay - PreviousDay) / Math.Abs(CurrentDay) * 100, 2);
}