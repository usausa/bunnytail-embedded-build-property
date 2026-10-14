namespace BunnyTail.EmbeddedBuildProperty.Tests;

using System.Reflection;

using BunnyTail.EmbeddedBuildProperty.Generator;

using Microsoft.CodeAnalysis;

public sealed class DiagnosticTests
{
    // ------------------------------------------------------------
    // Class (00xx)
    // ------------------------------------------------------------

    [Fact]
    public void Btbp0001InvalidNamespaceEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:abc", rootNamespace: "1Invalid.Namespace");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP0001");
    }

    [Fact]
    public void Btbp0002InvalidClassNameEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:abc", className: "1Invalid");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP0002");
    }

    // ------------------------------------------------------------
    // Entry (01xx)
    // ------------------------------------------------------------

    [Fact]
    public void Btbp0101NameSeparatorMissingEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("NoSeparator");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP0101");
    }

    [Fact]
    public void Btbp0102TypeSeparatorMissingEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=abc");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP0102");
    }

    [Fact]
    public void Btbp0103InvalidConstNameEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("1Value=string:abc");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP0103");
    }

    [Fact]
    public void Btbp0104UnsupportedTypeEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=DateTime:2026-01-01");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP0104");
    }

    [Fact]
    public void Btbp0105EmptyValueEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=int:");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP0105");
    }

    [Fact]
    public void Btbp0106InvalidValueEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=int:notanumber");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP0106");
    }

    [Fact]
    public void Btbp0107DuplicateNameEmitsDiagnostic()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=string:abc,Value1=string:xyz");

        Assert.Contains(diagnostics, static x => x.Id == "BTBP0107");
    }

    // ------------------------------------------------------------
    // Reporting
    // ------------------------------------------------------------

    [Fact]
    public void SameDiagnosticIsReportedOnce()
    {
        var diagnostics = GeneratorTestHelper.GetDiagnostics("Value1=int:notanumber,Value1=int:notanumber");

        Assert.Single(diagnostics, static x => x.Id == "BTBP0106");
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
