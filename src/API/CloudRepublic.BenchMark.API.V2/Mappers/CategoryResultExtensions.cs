using CloudRepublic.BenchMark.API.V2.Models;
using CloudRepublic.BenchMark.Application.Models;

namespace CloudRepublic.BenchMark.API.V2.Mappers;

public static class CategoryResultExtensions
{
    public static IEnumerable<Category> ToCategories(this IEnumerable<BenchMarkType> benchMarkTypes)
    {
        return benchMarkTypes.Select(benchMarkType => new Category
            {
                Title = benchMarkType.Title,
                Cloud = benchMarkType.CloudProvider,
                Language = benchMarkType.Language,
                Os = benchMarkType.HostEnvironment,
                Runtime = benchMarkType.Runtime,
                Sku = benchMarkType.Sku
            })
            .ToList();
    }
}