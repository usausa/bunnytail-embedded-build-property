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
    public void GeneratedClassIsInternalByDefault()
    {
        var result = GeneratorTestHelper.Run("Value1=string:abc", "// no user code");

        Assert.Empty(result.Problems);
        Assert.Equal(Accessibility.Internal, result.OutputCompilation.GetTypeByMetadataName("EmbeddedProperty")!.DeclaredAccessibility);
    }

    [Fact]
    public void GeneratedClassFollowsUserAccessibility()
    {
        var result = GeneratorTestHelper.Run("Value1=string:abc", "public static partial class EmbeddedProperty { }");

        Assert.Empty(result.Problems);
        Assert.Equal(Accessibility.Public, result.OutputCompilation.GetTypeByMetadataName("EmbeddedProperty")!.DeclaredAccessibility);
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

    //-----------------------------------------------------------------------
    // Values
    //-----------------------------------------------------------------------

    [Fact]
    public void EscapedCharactersAreRestored()
    {
        var generated = GeneratorTestHelper.GetGeneratedSource("Hash=string:a%23b,Semi=string:c%3Bd,Percent=string:e%25f");

        Assert.Contains("Hash = @\"a#b\";", generated, StringComparison.Ordinal);
        Assert.Contains("Semi = @\"c;d\";", generated, StringComparison.Ordinal);
        Assert.Contains("Percent = @\"e%f\";", generated, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("Value=float:5.", "5f")]
    [InlineData("Value=double:5.", "5d")]
    [InlineData("Value=double:1e3", "1000d")]
    [InlineData("Value=decimal:0.10", "0.10m")]
    public void NumberIsWrittenAsLiteral(string values, string literal)
    {
        var generated = GeneratorTestHelper.GetGeneratedSource(values);

        Assert.Empty(GeneratorTestHelper.GetProblemIds(values));
        Assert.Contains("Value = " + literal + ";", generated, StringComparison.Ordinal);
    }
}
