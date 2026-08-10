namespace BunnyTail.EmbeddedBuildProperty.Tests;

using Microsoft.CodeAnalysis;

public class BuildPropertyGeneratorTest
{
    // Entry format is "Name=type:value", entries separated by ',' (backslash escapes a literal ',').

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
    // BTBP1001 : invalid namespace
    //-----------------------------------------------------------------------

    [Fact]
    public void Btbp1001InvalidNamespaceEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:abc", rootNamespace: "1Invalid.Namespace");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1001");
    }

    //-----------------------------------------------------------------------
    // BTBP1002 : invalid class name
    //-----------------------------------------------------------------------

    [Fact]
    public void Btbp1002InvalidClassNameEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:abc", className: "1Invalid");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1002");
    }

    //-----------------------------------------------------------------------
    // BTBP1003 : '=' separator missing
    //-----------------------------------------------------------------------

    [Fact]
    public void Btbp1003NameSeparatorMissingEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("NoSeparator");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1003");
    }

    //-----------------------------------------------------------------------
    // BTBP1004 : ':' separator missing
    //-----------------------------------------------------------------------

    [Fact]
    public void Btbp1004TypeSeparatorMissingEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=abc");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1004");
    }

    //-----------------------------------------------------------------------
    // BTBP1005 : invalid constant name
    //-----------------------------------------------------------------------

    [Fact]
    public void Btbp1005InvalidConstNameEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("1Value=string:abc");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1005");
    }

    //-----------------------------------------------------------------------
    // BTBP1006 : unsupported constant type
    //-----------------------------------------------------------------------

    [Fact]
    public void Btbp1006UnsupportedTypeEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=DateTime:2026-01-01");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1006");
    }

    //-----------------------------------------------------------------------
    // BTBP1007 : empty value for a non-string type
    //-----------------------------------------------------------------------

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

    //-----------------------------------------------------------------------
    // BTBP1008 : value does not parse as the declared type
    //-----------------------------------------------------------------------

    [Fact]
    public void Btbp1008InvalidValueEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=int:notanumber");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1008");
    }

    //-----------------------------------------------------------------------
    // BTBP1009 : duplicated constant name
    //-----------------------------------------------------------------------

    [Fact]
    public void Btbp1009DuplicateNameEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:abc,Value1=string:xyz");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP1009");
    }
}
