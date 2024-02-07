namespace CloudRepublic.BenchMark.Application.Models
{
    public record BenchMarkResponse(bool Success, int StatusCode, long Duration, string ServerName, int CallPositionNumber);
}