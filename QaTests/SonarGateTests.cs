using Altinn.Dd.Tests.SonarGate;
using Xunit;
using Xunit.Abstractions;

namespace QaTests;

// Opt-in SonarQube quality-gate test for oed-testdata-app. The actual runner lives in the
// Altinn.Dd.Tests.SonarGate package — this file is just the option blob. See
// https://altinn.studio/repos/digdir/dd-qa for the package source.
//
// Scope: oed-testdata.Server (the main backend). The JsonGenerator utility in the same repo
// isn't scanned by this run — add a separate SonarGate test if you want it on the dashboard.
//
// Run with:  $env:QATESTS = "1"; dotnet test ./QaTests/QaTests.csproj
public class SonarGateTests(ITestOutputHelper output)
{
    [SkippableFact, Trait("Category", "qa")]
    public Task QualityGate_ReturnsOk() => SonarGate.RunAsync(new()
    {
        ProjectKey = "oed-testdata-app",
        ScanCsprojRelativePath = "src/oed-testdata.Server/oed-testdata.Server.csproj",
    }, output);
}
