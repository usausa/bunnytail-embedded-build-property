namespace BunnyTail.EmbeddedBuildProperty.Tests;

using Microsoft.CodeAnalysis;

public class BuildPropertyGeneratorTest
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
    public void Btbp1001InvalidNamespaceEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:abc", rootNamespace: "1Invalid.Namespace");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1001");
    }

    [Fact]
    public void Btbp1002InvalidClassNameEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:abc", className: "1Invalid");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1002");
    }

    [Fact]
    public void Btbp1003NameSeparatorMissingEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("NoSeparator");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1003");
    }

    [Fact]
    public void Btbp1004TypeSeparatorMissingEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=abc");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1004");
    }

    [Fact]
    public void Btbp1005InvalidConstNameEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("1Value=string:abc");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1005");
    }

    [Fact]
    public void Btbp1006UnsupportedTypeEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=DateTime:2026-01-01");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1006");
    }

    [Fact]
    public void Btbp1007EmptyValueEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=int:");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1007");
    }

    [Fact]
    public void EmptyStringValueIsAllowed()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:");

        Assert.Empty(diagnostics);
    }

    [Fact]
    public void Btbp1008InvalidValueEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=int:notanumber");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1008");
    }

    [Fact]
    public void Btbp1009DuplicateNameEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:abc,Value1=string:xyz");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1009");
    }
}
