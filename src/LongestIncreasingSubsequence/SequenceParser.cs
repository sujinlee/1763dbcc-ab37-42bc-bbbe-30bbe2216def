using System.Globalization;

namespace LongestIncreasingSubsequence;

/// <summary>
/// Converts a whitespace-separated string of integers into numbers.
/// </summary>
public static class SequenceParser
{
    /// <summary>
    /// Parses <paramref name="input"/> into a list of integers.
    /// Leading/trailing whitespace is ignored, so input read from a file or stdin
    /// (which usually ends with a newline) is accepted.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="input"/> is null.</exception>
    /// <exception cref="FormatException">A token is not a valid integer.</exception>
    public static IReadOnlyList<long> Parse(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        var tokens = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
        var numbers = new long[tokens.Length];

        for (var i = 0; i < tokens.Length; i++)
        {
            if (!long.TryParse(tokens[i], NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out numbers[i]))
            {
                throw new FormatException($"'{tokens[i]}' at position {i + 1} is not a valid integer.");
            }
        }

        return numbers;
    }
}
