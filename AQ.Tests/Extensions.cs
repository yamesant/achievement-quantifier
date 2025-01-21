using System.Text.RegularExpressions;

namespace AQ.Tests;

public static class Extensions
{
    public static string RemoveWhitespace(this string input)
    {
        return Regex.Replace(input, @"\s+", "");
    }
}