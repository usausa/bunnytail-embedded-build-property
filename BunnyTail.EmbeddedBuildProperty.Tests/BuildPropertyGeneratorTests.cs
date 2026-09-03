namespace BunnyTail.EmbeddedBuildProperty.Tests;

using Microsoft.CodeAnalysis;

public class BuildPropertyGeneratorTests
{
    //-----------------------------------------------------------------------
    // Basic
    //-----------------------------------------------------------------------

    [Fact]
    public void BasicValuesGenerateConstants()
    {
        var generated = GeneratorTestHelper.GetGeneratedSource("Value1=string:abc,Value2=int:123");

        Assert.Contains("Value1", generated, StringComparison.Ordinal);
        Assert.Contains("\"abc\"", generated, StringComparison.Ordinal);
        Assert.Contains("Value2", generated, StringComparison.Ordinal);
        Assert.Contains("123", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void BasicValuesProduceNoCompilationError()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnosticsAll("Value1=string:abc,Value2=int:123");

        Assert.DoesNotContain(diagnostics, static x => x.Severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public void ClassNameOptionIsHonored()
    {
        var generated = GeneratorTestHelper.GetGeneratedSource("Value1=string:abc", className: "MySettings");

        Assert.Contains("MySettings", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void RootNamespaceOptionIsHonored()
    {
        var generated = GeneratorTestHelper.GetGeneratedSource("Value1=string:abc", rootNamespace: "My.App");

        Assert.Contains("namespace My.App", generated, StringComparison.Ordinal);
    }

    [Fact]
    public void ValidDefinitionEmitsNoDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:abc,Value2=int:123,Value3=bool:true");

        Assert.Empty(diagnostics);
    }

    //-----------------------------------------------------------------------
    // Diagnostics
    //-----------------------------------------------------------------------

    [Fact]
    public void EmptyStringValueIsAllowed()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:");

        Assert.Empty(diagnostics);
    }
}
