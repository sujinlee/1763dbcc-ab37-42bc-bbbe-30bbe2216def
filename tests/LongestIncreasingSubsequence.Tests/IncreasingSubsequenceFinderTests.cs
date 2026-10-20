namespace LongestIncreasingSubsequence.Tests;

public class IncreasingSubsequenceFinderTests
{
    private static readonly string TestCasesDirectory = Path.Combine(AppContext.BaseDirectory, "TestCases");

    /// <summary>
    /// The eleven test cases from the specification, stored as
    /// <c>TestCases/case-NN.input.txt</c> and <c>TestCases/case-NN.expected.txt</c>.
    /// </summary>
    public static TheoryData<string> SpecificationCases()
    {
        var data = new TheoryData<string>();
        foreach (var inputFile in Directory.GetFiles(TestCasesDirectory, "*.input.txt").Order())
        {
            data.Add(Path.GetFileName(inputFile).Replace(".input.txt", string.Empty, StringComparison.Ordinal));
        }

        return data;
    }

    [Fact]
    public void SpecificationCases_AllElevenArePresent()
    {
        Assert.Equal(11, SpecificationCases().Count);
    }

    [Theory]
    [MemberData(nameof(SpecificationCases))]
    public void Find_SpecificationCase_ReturnsExpectedOutput(string caseName)
    {
        var input = File.ReadAllText(Path.Combine(TestCasesDirectory, $"{caseName}.input.txt"));
        var expected = File.ReadAllText(Path.Combine(TestCasesDirectory, $"{caseName}.expected.txt")).Trim();

        var actual = IncreasingSubsequenceFinder.Find(input);

        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("6 1 5 9 2", "1 5 9")]
    [InlineData("6 2 4 6 1 5 9 2", "2 4 6")]
    [InlineData("6 2 4 3 1 5 9", "1 5 9")]
    public void Find_ShortSpecificationCases_ReturnsExpectedOutput(string input, string expected)
    {
        Assert.Equal(expected, IncreasingSubsequenceFinder.Find(input));
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("   ", "")]
    [InlineData("7", "7")]
    [InlineData("1 2 3 4 5", "1 2 3 4 5")]
    public void Find_TrivialInputs_ReturnsExpectedOutput(string input, string expected)
    {
        Assert.Equal(expected, IncreasingSubsequenceFinder.Find(input));
    }

    [Fact]
    public void Find_StrictlyDecreasing_ReturnsFirstNumber()
    {
        Assert.Equal("5", IncreasingSubsequenceFinder.Find("5 4 3 2 1"));
    }

    [Fact]
    public void Find_AllEqual_ReturnsFirstNumber()
    {
        Assert.Equal("3", IncreasingSubsequenceFinder.Find("3 3 3"));
    }

    [Fact]
    public void Find_EqualNeighbours_BreakTheRun()
    {
        // "Increasing" is strict: 2 -> 2 does not continue the run.
        Assert.Equal("2 5 8", IncreasingSubsequenceFinder.Find("1 2 2 5 8"));
    }

    [Fact]
    public void Find_TiedLongestRuns_ReturnsEarliest()
    {
        Assert.Equal("4 5 6", IncreasingSubsequenceFinder.Find("4 5 6 3 1 2 3 2 7 8"));
    }

    [Fact]
    public void Find_LongestRunAtEnd_ReturnsIt()
    {
        Assert.Equal("0 1 2 3", IncreasingSubsequenceFinder.Find("5 6 1 0 1 2 3"));
    }

    [Fact]
    public void Find_NegativeNumbers_AreSupported()
    {
        Assert.Equal("-10 -5 0 5", IncreasingSubsequenceFinder.Find("3 -10 -5 0 5 -1"));
    }

    [Fact]
    public void Find_ValuesBeyondInt32_AreSupported()
    {
        Assert.Equal("2147483647 9999999999", IncreasingSubsequenceFinder.Find("2147483647 9999999999 1"));
    }

    [Fact]
    public void Find_TrailingNewline_IsIgnored()
    {
        Assert.Equal("1 5 9", IncreasingSubsequenceFinder.Find("6 1 5 9 2\n"));
    }

    [Fact]
    public void Find_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => IncreasingSubsequenceFinder.Find(null!));
    }

    [Fact]
    public void FindLongestIncreasingRun_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => IncreasingSubsequenceFinder.FindLongestIncreasingRun(null!));
    }

    [Fact]
    public void FindLongestIncreasingRun_Empty_ReturnsEmpty()
    {
        Assert.Empty(IncreasingSubsequenceFinder.FindLongestIncreasingRun([]));
    }

    [Fact]
    public void FindLongestIncreasingRun_ReturnsNumbersInOriginalOrder()
    {
        long[] numbers = [9, 1, 4, 7, 2];

        Assert.Equal([1L, 4L, 7L], IncreasingSubsequenceFinder.FindLongestIncreasingRun(numbers));
    }
}
