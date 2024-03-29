using System.Text.Json.Serialization;
using CloudRepublic.BenchMark.API.V2.Models;

namespace CloudRepublic.BenchMark.API.V2.Serializers;

[JsonSourceGenerationOptions(WriteIndented = false, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(BenchMarkData))]
[JsonSerializable(typeof(BenchmarkMedians))]
[JsonSerializable(typeof(Category))]
[JsonSerializable(typeof(DataPoint))]
public partial class StatisticsSerializerContext : JsonSerializerContext;