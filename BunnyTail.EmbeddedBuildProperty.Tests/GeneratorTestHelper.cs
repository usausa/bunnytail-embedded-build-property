namespace BunnyTail.EmbeddedBuildProperty.Tests;

using System.Collections.Generic;

using BunnyTail.EmbeddedBuildProperty.Generator;

using Microsoft.CodeAnalysis;

using SourceGenerateHelper.Testing;

internal static class GeneratorTestHelper
{
    private const string EmptySource = "// no user code";

    private static GeneratorTestRunner CreateRunner(string? values, string? className = null, string? rootNamespace = null)
    {
        var runner = GeneratorTestRunner
            .For<BuildPropertyGenerator>()
            .WithDiagnosticPrefix("BTBP");

        if (rootNamespace is not null)
        {
            runner.WithGlobalOption("build_property.RootNamespace", rootNamespace);
        }

        if (className is not null)
        {
            runner.WithGlobalOption("build_property.EmbeddedPropertyClass", className);
        }

        if (values is not null)
        {
            runner.WithGlobalOption("build_property._EmbeddedPropertyValues", values);
        }

        return runner;
    }

    public static IReadOnlyList<Diagnostic> GetDiagnostics(string? values, string? className = null, string? rootNamespace = null) =>
        CreateRunner(values, className, rootNamespace).GetDiagnostics(EmptySource);

    public static IReadOnlyList<Diagnostic> GetDiagnosticsAll(string? values, string? className = null, string? rootNamespace = null) =>
        CreateRunner(values, className, rootNamespace).GetDiagnosticsAll(EmptySource);

    public static string GetGeneratedSource(string? values, string? className = null, string? rootNamespace = null) =>
        CreateRunner(values, className, rootNamespace).GetGeneratedSource(EmptySource);
}
