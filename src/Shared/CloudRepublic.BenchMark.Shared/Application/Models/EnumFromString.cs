using System;

namespace CloudRepublic.BenchMark.Application.Models;

/// <summary>
/// <para> An Itty Bitty Helper to parse strings to enums. </para>
/// <para> Verify the '<see cref="ParsedSuccesfull"/>' property whether the string could become the Enum on '<see cref="Value"/>' </para>
/// </summary>
/// <typeparam name="T"></typeparam>
public class EnumFromString<T> where T : struct
{
    public EnumFromString(string input)
    {
        StringValue = input;
        if (string.IsNullOrEmpty(StringValue))
        {
            ParsedSuccesfull = false;
        }

        T tempValue;
        ParsedSuccesfull = Enum.TryParse(StringValue, out tempValue);
        if (ParsedSuccesfull)
        {
            ParsedSuccesfull = Enum.IsDefined(typeof(T), tempValue);
            if (ParsedSuccesfull)
            {
                Value = tempValue;
            }
        }

    }
    public string StringValue { get; }
    public T Value { get; }
    public bool ParsedSuccesfull { get; }
}