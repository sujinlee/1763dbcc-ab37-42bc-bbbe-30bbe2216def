namespace LongestIncreasingSubsequence.Cli;

/// <summary>
/// Command-line entry logic, kept separate from <c>Program</c> so it can be unit tested
/// with in-memory readers and writers.
/// </summary>
public static class CommandLineApp
{
    /// <summary>Exit code when the subsequence was written to the output.</summary>
    public const int Success = 0;

    /// <summary>Exit code when the input contained something other than integers.</summary>
    public const int InvalidInput = 1;

    /// <summary>
    /// Reads the sequence from the arguments (joined with spaces) or, when there are none,
    /// from the first line of standard input, then writes the longest increasing subsequence.
    /// Reading a single line means typing the sequence and pressing Enter is enough.
    /// </summary>
    public static int Run(string[] args, TextReader input, TextWriter output, TextWriter error)
    {
        var sequence = args.Length > 0 ? string.Join(' ', args) : input.ReadLine() ?? string.Empty;

        try
        {
            output.WriteLine(IncreasingSubsequenceFinder.Find(sequence));
            return Success;
        }
        catch (FormatException ex)
        {
            error.WriteLine($"Invalid input: {ex.Message}");
            return InvalidInput;
        }
    }
}
