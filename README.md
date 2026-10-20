# Longest Increasing Subsequence

A C# .NET 10 solution that takes a string of integers separated by single spaces and returns the
longest increasing subsequence found in it. When more than one subsequence has the longest length,
the earliest one is returned.

```
Input:  6 1 5 9 2
Output: 1 5 9
```

## How the problem is interpreted

Every expected output in the specification is a run of **adjacent** numbers from the input. For
example, `6 2 4 3 1 5 9` gives `1 5 9`, not the longer non-adjacent `2 4 5 9`. So the solution
finds the longest run of consecutive, **strictly** increasing numbers (`2 2` does not continue a
run).

Other behaviour:

| Input | Result |
| --- | --- |
| Empty or whitespace-only string | Empty string |
| A single number | That number |
| No increasing pair (e.g. `5 4 3`) | The first number |
| Leading/trailing whitespace or a trailing newline | Ignored |
| Negative numbers or values beyond 32-bit range | Supported (64-bit integers) |
| A token that is not an integer (e.g. `1 x 3`) | `FormatException` naming the token and its position |

## Project layout

```
src/
  LongestIncreasingSubsequence/        Class library with the solution
    IncreasingSubsequenceFinder.cs     Find(string) – the function the problem asks for
    SequenceParser.cs                  Turns the input string into numbers
  LongestIncreasingSubsequence.Cli/    Console app (`lis`) wrapping the library
tests/
  LongestIncreasingSubsequence.Tests/  xUnit tests
    TestCases/                         The 11 test cases from the specification, one file each
```

The algorithm is a single O(n) pass: it tracks where the current increasing run started, resets
the start whenever a number is not greater than the one before it, and only replaces the best run
when a run is *strictly* longer – which is what makes the earliest run win ties.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Optional: [Docker](https://www.docker.com/) to run the containerised version

## Verifying the solution

### 1. Run the tests

```bash
dotnet test
```

This runs 50 tests, including all 11 test cases from the specification (the input and expected
output for each are in `tests/LongestIncreasingSubsequence.Tests/TestCases`).

### 2. Run it yourself

Pass the sequence as an argument:

```bash
dotnet run --project src/LongestIncreasingSubsequence.Cli -- "6 1 5 9 2"
```

Or run it without arguments for interactive mode: type a sequence, press Enter to see the result,
and keep entering sequences. Press `Ctrl+C` to exit.

```bash
dotnet run --project src/LongestIncreasingSubsequence.Cli
```

```
Enter integers separated by spaces and press Enter. Press Ctrl+C to exit.
1 5 68 59 45 2 8 4 26 85 47 25
1 5 68
6 2 4 3 1 5 9
1 5 9
```

Piped input is processed the same way, one result per line, which is handy for the large test
cases:

```bash
cat tests/LongestIncreasingSubsequence.Tests/TestCases/case-09.input.txt | dotnet run --project src/LongestIncreasingSubsequence.Cli
```

An invalid line (e.g. `1 x 3`) prints an error to stderr and the next line is still processed.
The CLI exits with code `0` when every sequence was valid and `1` otherwise.

### 3. Run it in Docker

The image build also runs the tests, so a failing test fails the build.

```bash
docker build -t lis .
```

```bash
docker run --rm lis "6 1 5 9 2"
```

```bash
docker run --rm -i lis < tests/LongestIncreasingSubsequence.Tests/TestCases/case-02.input.txt
```

## Code quality

### Linting

- .NET analyzers run on every build (`AnalysisLevel` = `latest-recommended`) and code-style rules
  from `.editorconfig` are enforced at build time. Warnings are treated as errors
  (see `Directory.Build.props`).
- Formatting is checked with:

  ```bash
  dotnet format --verify-no-changes
  ```

### Code coverage

Coverage is collected with Coverlet:

```bash
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults
```

This writes a Cobertura `coverage.cobertura.xml` under `TestResults/`. To turn it into an HTML
report locally:

```bash
dotnet tool install --global dotnet-reportgenerator-globaltool
```

```bash
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:CoverageReport -reporttypes:Html
```

The library and CLI logic have 100% line and branch coverage; the only uncovered line is the
one-line `Program.cs` entry point, which just forwards to the tested `CommandLineApp`.

## Continuous integration

`.github/workflows/ci.yml` runs on every push and pull request to `main`:

1. **Lint** – `dotnet format --verify-no-changes`
2. **Build** – Release build with analyzers and warnings as errors
3. **Test** – all tests with coverage collection
4. **Coverage report** – generated with ReportGenerator, added to the job summary and uploaded
   (with the test results) as a build artifact
5. **Docker** – builds the image and smoke-tests it against test case 1
