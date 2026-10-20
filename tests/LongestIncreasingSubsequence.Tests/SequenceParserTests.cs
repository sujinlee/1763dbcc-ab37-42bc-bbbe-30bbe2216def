namespace LongestIncreasingSubsequence.Tests;

public class SequenceParserTests
{
    [Fact]
    public void Parse_SpaceSeparatedIntegers_ReturnsNumbersInOrder()
    {
        Assert.Equal([6L, 1L, 5L, 9L, 2L], SequenceParser.Parse("6 1 5 9 2"));
    }

    [Fact]
    public void Parse_EmptyString_ReturnsEmpty()
    {
        Assert.Empty(SequenceParser.Parse(string.Empty));
    }

    [Fact]
    public void Parse_SurroundingWhitespace_IsIgnored()
    {
        Assert.Equal([1L, 2L], SequenceParser.Parse("  1 2\r\n"));
    }

    [Fact]
    public void Parse_SignedNumbers_AreParsed()
    {
        Assert.Equal([-3L, 4L], SequenceParser.Parse("-3 +4"));
    }

    [Theory]
    [InlineData("1 two 3", "'two' at position 2")]
    [InlineData("1 2.5", "'2.5' at position 2")]
    [InlineData("1,2", "'1,2' at position 1")]
    [InlineData("99999999999999999999", "'99999999999999999999' at position 1")]
    public void Parse_InvalidToken_ThrowsWithPosition(string input, string expectedMessageFragment)
    {
        var exception = Assert.Throws<FormatException>(() => SequenceParser.Parse(input));

        Assert.Contains(expectedMessageFragment, exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Parse_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => SequenceParser.Parse(null!));
    }
}
