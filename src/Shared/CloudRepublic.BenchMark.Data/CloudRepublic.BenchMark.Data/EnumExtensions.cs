using System;

namespace CloudRepublic.BenchMark.Data;

public static class EnumExtensions
{
    /// <summary>
    /// Get the name of a enum member (Shorthand for Enum.GetName)
    /// </summary>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Enum member name, or null if member is unknown</returns>
    public static string? GetName<T>(this T value) where T : Enum
    {
        return Enum.GetName(typeof(T), value);
    }
}