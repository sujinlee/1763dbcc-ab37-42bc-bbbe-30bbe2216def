using LongestIncreasingSubsequence.Cli;

namespace LongestIncreasingSubsequence.Tests;

public sealed class CommandLineAppTests : IDisposable
{
    private readonly StringWriter _output = new();
    private readonly StringWriter _error = new();

    public void Dispose()
    {
        _output.Dispose();
        _error.Dispose();
    }

    [Fact]
    public void Run_SequenceAsSingleArgument_WritesResult()
    {
        var exitCode = Run(["6 1 5 9 2"]);

        Assert.Equal(CommandLineApp.Success, exitCode);
        Assert.Equal("1 5 9", _output.ToString().Trim());
        Assert.Empty(_error.ToString());
    }

    [Fact]
    public void Run_SequenceAsSeparateArguments_WritesResult()
    {
        var exitCode = Run(["6", "1", "5", "9", "2"]);

        Assert.Equal(CommandLineApp.Success, exitCode);
        Assert.Equal("1 5 9", _output.ToString().Trim());
    }

    [Fact]
    public void Run_NoArguments_ReadsStandardInput()
    {
        var exitCode = Run([], standardInput: "6 2 4 6 1 5 9 2\n");

        Assert.Equal(CommandLineApp.Success, exitCode);
        Assert.Equal("2 4 6", _output.ToString().Trim());
    }

    [Fact]
    public void Run_NoArguments_WritesOneResultPerLineUntilInputEnds()
    {
        var exitCode = Run([], standardInput: "1 5 68 59 45 2 8 4 26 85 47 25\n6 1 5 9 2\n");

        Assert.Equal(CommandLineApp.Success, exitCode);
        Assert.Equal(["1 5 68", "1 5 9"], OutputLines());
    }

    [Fact]
    public void Run_NoArguments_SkipsBlankLines()
    {
        var exitCode = Run([], standardInput: "\n   \n6 1 5 9 2\n\n");

        Assert.Equal(CommandLineApp.Success, exitCode);
        Assert.Equal(["1 5 9"], OutputLines());
    }

    [Fact]
    public void Run_NoArguments_InvalidLineIsReportedAndLaterLinesStillRun()
    {
        var exitCode = Run([], standardInput: "1 x 3\n6 1 5 9 2\n");

        Assert.Equal(CommandLineApp.InvalidInput, exitCode);
        Assert.Equal(["1 5 9"], OutputLines());
        Assert.StartsWith("Invalid input: 'x' at position 2", _error.ToString(), StringComparison.Ordinal);
    }

    [Fact]
    public void Run_NoArgumentsAndEmptyInput_WritesNothing()
    {
        var exitCode = Run([]);

        Assert.Equal(CommandLineApp.Success, exitCode);
        Assert.Empty(_output.ToString());
    }

    [Fact]
    public void Run_Interactive_ShowsPromptOnStandardError()
    {
        var exitCode = Run([], standardInput: "6 1 5 9 2\n", isInteractive: true);

        Assert.Equal(CommandLineApp.Success, exitCode);
        Assert.Equal(CommandLineApp.InteractivePrompt, _error.ToString().Trim());
        Assert.Equal(["1 5 9"], OutputLines());
    }

    [Fact]
    public void Run_WithArguments_DoesNotReadStandardInputOrShowPrompt()
    {
        var exitCode = Run(["6 1 5 9 2"], standardInput: "1 2 3\n", isInteractive: true);

        Assert.Equal(CommandLineApp.Success, exitCode);
        Assert.Equal(["1 5 9"], OutputLines());
        Assert.Empty(_error.ToString());
    }

    [Fact]
    public void Run_InvalidInput_WritesErrorAndReturnsFailure()
    {
        var exitCode = Run(["1 x 3"]);

        Assert.Equal(CommandLineApp.InvalidInput, exitCode);
        Assert.Empty(_output.ToString());
        Assert.StartsWith("Invalid input: 'x' at position 2", _error.ToString(), StringComparison.Ordinal);
    }

    private int Run(string[] args, string standardInput = "", bool isInteractive = false)
    {
        using var input = new StringReader(standardInput);
        return CommandLineApp.Run(args, input, _output, _error, isInteractive);
    }

    private string[] OutputLines() =>
        _output.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
}
