namespace LongestIncreasingSubsequence.Cli;

/// <summary>
/// Command-line entry logic, kept separate from <c>Program</c> so it can be unit tested
/// with in-memory readers and writers.
/// </summary>
public static class CommandLineApp
{
    /// <summary>Exit code when every sequence was processed.</summary>
    public const int Success = 0;

    /// <summary>Exit code when at least one input contained something other than integers.</summary>
    public const int InvalidInput = 1;

    /// <summary>Hint shown once when the user types sequences interactively.</summary>
    public const string InteractivePrompt = "Enter integers separated by spaces and press Enter. Press Ctrl+C to exit.";

    /// <summary>
    /// With arguments, treats them (joined with spaces) as one sequence and writes its longest
    /// increasing subsequence. Without arguments, does the same for every line of standard input
    /// until the input ends or the user presses Ctrl+C.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <param name="input">Standard input.</param>
    /// <param name="output">Standard output; receives one result per sequence.</param>
    /// <param name="error">Standard error; receives the interactive hint and validation errors.</param>
    /// <param name="isInteractive">True when a person is typing, so the hint is shown.</param>
    public static int Run(string[] args, TextReader input, TextWriter output, TextWriter error, bool isInteractive = false)
    {
        if (args.Length > 0)
        {
            return Process(string.Join(' ', args), output, error) ? Success : InvalidInput;
        }

        if (isInteractive)
        {
            error.WriteLine(InteractivePrompt);
        }

        var exitCode = Success;
        while (input.ReadLine() is { } line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (!Process(line, output, error))
            {
                exitCode = InvalidInput;
            }
        }

        return exitCode;
    }

    private static bool Process(string sequence, TextWriter output, TextWriter error)
    {
        try
        {
            output.WriteLine(IncreasingSubsequenceFinder.Find(sequence));
            return true;
        }
        catch (FormatException ex)
        {
            error.WriteLine($"Invalid input: {ex.Message}");
            return false;
        }
    }
}
