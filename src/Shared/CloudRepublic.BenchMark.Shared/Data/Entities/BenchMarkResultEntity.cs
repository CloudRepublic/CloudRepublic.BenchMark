using System;
using Azure;
using Azure.Data.Tables;

namespace CloudRepublic.BenchMark.Data.Entities;

public record BenchMarkResultEntity : ITableEntity
{
    public string? PartitionKey { get; set; }
    public string? RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string? RecordJson { get; set; }
}