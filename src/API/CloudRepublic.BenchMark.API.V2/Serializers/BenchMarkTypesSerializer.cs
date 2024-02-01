using System.Text.Json.Serialization;
using CloudRepublic.BenchMark.Application.Models;

namespace CloudRepublic.BenchMark.API.V2.Serializers;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(BenchMarkType))]
public partial class BenchMarkTypesSerializer : JsonSerializerContext;