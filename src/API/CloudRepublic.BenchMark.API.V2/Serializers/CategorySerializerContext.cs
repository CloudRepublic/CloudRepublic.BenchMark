using System.Text.Json.Serialization;
using CloudRepublic.BenchMark.API.V2.Models;

namespace CloudRepublic.BenchMark.API.V2.Serializers;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(Category))]
[JsonSerializable(typeof(IEnumerable<Category>))]
public partial class CategorySerializerContext : JsonSerializerContext;