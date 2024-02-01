using System.Text.Json.Serialization;
using CloudRepublic.BenchMark.Domain.Entities;

namespace CloudRepublic.BenchMark.Data.Serializers;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(BenchMarkResult))]
public partial class BenchMarkResultSerializer : JsonSerializerContext;