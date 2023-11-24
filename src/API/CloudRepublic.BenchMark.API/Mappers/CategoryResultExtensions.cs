using System.Collections.Generic;
using CloudRepublic.BenchMark.API.Models;
using CloudRepublic.BenchMark.Application.Models;

namespace CloudRepublic.BenchMark.API.Mappers;

public static class CategoryResultExtensions
{
    public static IEnumerable<Category> ToCategories(this IEnumerable<BenchMarkType> benchMarkTypes)
    {
        var categories = new List<Category>();

        foreach (var benchMarkType in benchMarkTypes)
        {
            categories.Add(new Category
            {
                Title = benchMarkType.Title,
                Cloud = benchMarkType.CloudProvider,
                Language = benchMarkType.Language,
                Os = benchMarkType.HostEnvironment,
                Runtime = benchMarkType.Runtime,
                Sku = benchMarkType.Sku
            });
        }

        return categories;
    }
}