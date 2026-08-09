# BunnyTail.EmbeddedBuildProperty

[![NuGet](https://img.shields.io/nuget/v/BunnyTail.EmbeddedBuildProperty.svg)](https://www.nuget.org/packages/BunnyTail.EmbeddedBuildProperty)

## What is this?

Generate consts class to get build options.

## Usage

### Project

```xml
  <PropertyGroup>
    <EmbeddedFlavor>Development</EmbeddedFlavor>
    <EmbeddedSecretKey></EmbeddedFlavor>
  </PropertyGroup>

  <Import Project="..\.UserEmbeddedProperty.props" Condition="Exists('..\.UserEmbeddedProperty.props')" />

  <PropertyGroup>
    <EmbeddedPropertyValues>
      Flavor=string:$(EmbeddedFlavor),
      SecretKey=string:$(EmbeddedSecretKey)
    </EmbeddedPropertyValues>
  </PropertyGroup>
```

Each entry is `Name=Type:Value`. Supported types are `string`, `bool`, `char`,
`byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `float`, `double` and `decimal`.

Values other than `string` are validated and normalized before they are emitted, and an invalid value is reported as `BTBP1008` instead of producing a broken generated file.

| Type | Accepted | Emitted |
|---|---|---|
| `bool` | `true` / `True` / `TRUE` and the false forms | `true` / `false` |
| integers | decimal notation within the range of the type | normalized (`+42` becomes `42`) |
| `float` / `double` / `decimal` | decimal notation, with or without the type suffix | suffixed (`f` / `d` / `m`) |
| `char` | exactly one character | quoted and escaped |

### Source

```cs
Console.WriteLine($"Flavor: {EmbeddedProperty.Flavor}");
Console.WriteLine($"SecretKey: {EmbeddedProperty.SecretKey}");
```

### Build

```
dotnet build Example.csproj /p:Flavor=Production /p:SecretKey=xxxx
```
