namespace LongestIncreasingSubsequence;

/// <summary>
/// Finds the longest run of consecutive, strictly increasing numbers in a sequence.
/// </summary>
/// <remarks>
/// The expected outputs in the specification are always adjacent elements of the input
/// (e.g. "6 2 4 3 1 5 9" gives "1 5 9", not "2 4 5 9"), so "subsequence" here means a
/// contiguous run. When several runs share the longest length, the earliest one wins.
/// </remarks>
public static class IncreasingSubsequenceFinder
{
    /// <summary>
    /// Takes a whitespace-separated string of integers and returns the longest
    /// increasing subsequence as a single-space-separated string.
    /// </summary>
    /// <example>
    /// <c>Find("6 1 5 9 2")</c> returns <c>"1 5 9"</c>.
    /// </example>
    /// <exception cref="ArgumentNullException"><paramref name="input"/> is null.</exception>
    /// <exception cref="FormatException"><paramref name="input"/> contains a non-integer token.</exception>
    public static string Find(string input)
    {
        var numbers = SequenceParser.Parse(input);
        var longestRun = FindLongestIncreasingRun(numbers);
        return string.Join(' ', longestRun);
    }

    /// <summary>
    /// Returns the longest strictly increasing run of adjacent numbers, preferring the
    /// earliest run on ties. Runs in O(n) time with a single pass.
    /// </summary>
    public static IReadOnlyList<long> FindLongestIncreasingRun(IReadOnlyList<long> numbers)
    {
        ArgumentNullException.ThrowIfNull(numbers);

        if (numbers.Count == 0)
        {
            return [];
        }

        var bestStart = 0;
        var bestLength = 1;
        var currentStart = 0;

        for (var i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] <= numbers[i - 1])
            {
                currentStart = i;
                continue;
            }

            var currentLength = i - currentStart + 1;

            // Strictly greater, so an equally long run found later never replaces the earliest one.
            if (currentLength > bestLength)
            {
                bestStart = currentStart;
                bestLength = currentLength;
            }
        }

        return numbers.Skip(bestStart).Take(bestLength).ToArray();
    }
}
