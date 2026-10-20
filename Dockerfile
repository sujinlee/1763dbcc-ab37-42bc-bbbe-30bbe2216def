# Build stage: restore, lint, test, then publish the CLI
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY Directory.Build.props .editorconfig LongestIncreasingSubsequence.slnx ./
COPY src/LongestIncreasingSubsequence/*.csproj src/LongestIncreasingSubsequence/
COPY src/LongestIncreasingSubsequence.Cli/*.csproj src/LongestIncreasingSubsequence.Cli/
COPY tests/LongestIncreasingSubsequence.Tests/*.csproj tests/LongestIncreasingSubsequence.Tests/
RUN dotnet restore LongestIncreasingSubsequence.slnx

COPY . .
RUN dotnet test LongestIncreasingSubsequence.slnx --configuration Release --no-restore
RUN dotnet publish src/LongestIncreasingSubsequence.Cli --configuration Release --no-restore --output /app

# Runtime stage: small image that only contains the published CLI
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["dotnet", "lis.dll"]
