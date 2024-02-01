using System.Text.Json.Serialization;
using CloudRepublic.BenchMark.API.V2.Models;
using CloudRepublic.BenchMark.Application.Models;

namespace CloudRepublic.BenchMark.API.V2.Serializers;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(Category))]
public partial class CategorySerializerContext : JsonSerializerContext;