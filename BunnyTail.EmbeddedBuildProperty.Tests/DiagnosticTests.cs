namespace BunnyTail.EmbeddedBuildProperty.Tests;

using System.Reflection;

using BunnyTail.EmbeddedBuildProperty.Generator;

using Microsoft.CodeAnalysis;

public sealed class DiagnosticTests
{
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

    [Fact]
    public void SameDiagnosticIsReportedOnce()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=int:notanumber,Value1=int:notanumber");

        Assert.Single(diagnostics, static x => x.Id == "BTBP1008");
    }

    [Fact]
    public void ErrorsCannotBeSuppressed()
    {
        var descriptors = typeof(BuildPropertyGenerator).Assembly.GetType("BunnyTail.EmbeddedBuildProperty.Generator.Diagnostics", throwOnError: true)!
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Where(static x => x.PropertyType == typeof(DiagnosticDescriptor))
            .Select(static x => (DiagnosticDescriptor)x.GetValue(null)!)
            .ToList();

        Assert.All(
            descriptors.Where(static x => x.DefaultSeverity == DiagnosticSeverity.Error),
            static x => Assert.Equal([WellKnownDiagnosticTags.NotConfigurable, WellKnownDiagnosticTags.Compiler], x.CustomTags));
    }
}
