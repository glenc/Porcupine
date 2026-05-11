using System.Text.RegularExpressions;

namespace Porcupine.WebApi.Infrastructure;

public static partial class StringExtensions
{
    public static string ToKebabCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        // Matches boundaries between lower/digits and upper case letters
        return MyRegex().Replace(input, "-$1")
                    .Trim()
                    .ToLowerInvariant();
    }

    [GeneratedRegex("(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])")]
    private static partial Regex MyRegex();
}