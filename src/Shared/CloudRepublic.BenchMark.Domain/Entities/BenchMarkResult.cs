using CloudRepublic.BenchMark.Domain.Enums;
using System;

namespace CloudRepublic.BenchMark.Domain.Entities;

public record BenchMarkResult
{
    public string Id { get; set; }
    public CloudProvider CloudProvider { get; set; }
    public HostEnvironment HostingEnvironment { get; set; }
    public Runtime Runtime { get; set; }
    public Language Language { get; set; }
    public string Sku { get; set; }
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public int RequestDuration { get; set; }
    /// <summary>
    /// This holds the position in the run benchmark que. 
    /// </summary>
    public int CallPositionNumber { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
        
    public bool IsColdRequest { get; set; }
}