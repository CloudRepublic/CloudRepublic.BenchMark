namespace CloudRepublic.BenchMark.API.V2.Mappers;

public static class LinqExtensions
{
    public static double Median(this IEnumerable<double> source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var sortedList = source.OrderBy(x => x).ToList();
        var count = sortedList.Count;
        if (count == 0)
        {
            throw new InvalidOperationException("The source sequence is empty.");
        }

        if (count % 2 == 0)
        {
            return (sortedList[count / 2 - 1] + sortedList[count / 2]) / 2.0;
        }

        return sortedList[count / 2];
    }
}