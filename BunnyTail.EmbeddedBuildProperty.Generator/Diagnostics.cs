namespace BunnyTail.EmbeddedBuildProperty.Generator;

using Microsoft.CodeAnalysis;

internal static class Diagnostics
{
    // Property

    public static DiagnosticDescriptor InvalidPropertyDefinition { get; } = new(
        id: "BTBP0001",
        title: "Invalid property definition",
        messageFormat: "Property must be static partial and has getter. property=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor UnsupportedPropertyType { get; } = new(
        id: "BTBP0002",
        title: "Unsupported property type",
        messageFormat: "Unsupported property type. type=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    // Constant

    public static DiagnosticDescriptor InvalidConstValueName { get; } = new(
        id: "BTBP1001",
        title: "Invalid const value name",
        messageFormat: "Name separator '=' is not found. entry=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidConstValueType { get; } = new(
        id: "BTBP1002",
        title: "Invalid const value type",
        messageFormat: "Type separator ':' is not found. entry=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor EmptyConstValue { get; } = new(
        id: "BTBP1003",
        title: "Empty const value",
        messageFormat: "Value for a non-string const is empty. name=[{0}], type=[{1}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidConstName { get; } = new(
        id: "BTBP1004",
        title: "Invalid const name",
        messageFormat: "Const name is not a valid identifier. name=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor UnsupportedConstType { get; } = new(
        id: "BTBP1005",
        title: "Unsupported const type",
        messageFormat: "Const type is not supported. type=[{0}], name=[{1}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor DuplicateConstName { get; } = new(
        id: "BTBP1006",
        title: "Duplicate const name",
        messageFormat: "Const name is duplicated. name=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidNamespace { get; } = new(
        id: "BTBP1007",
        title: "Invalid namespace",
        messageFormat: "Namespace is not valid. namespace=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);

    public static DiagnosticDescriptor InvalidClassName { get; } = new(
        id: "BTBP1008",
        title: "Invalid class name",
        messageFormat: "Class name is not a valid identifier. className=[{0}]",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true);
}
