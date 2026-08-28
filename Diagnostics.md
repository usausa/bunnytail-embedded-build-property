# Diagnostics

| ID | Severity | Description | How to fix |
|---|---|---|---|
| BTBP1001 | ❌ Error | Generated namespace is not a valid namespace | Give `EmbeddedBuildPropertyNamespace` a valid namespace |
| BTBP1002 | ❌ Error | Generated class name is not a valid identifier | Give `EmbeddedBuildPropertyClass` a valid identifier |
| BTBP1003 | ⚠️ Warning | Entry has no `=` separating the name from the value | Write the entry as `Name=Value` |
| BTBP1004 | ⚠️ Warning | Entry has no `:` separating the name from the type | Write the entry as `Name:Type=Value` |
| BTBP1005 | ⚠️ Warning | Const name is not a valid identifier | Rename the entry to a valid C# identifier |
| BTBP1006 | ⚠️ Warning | Const type is not one of the supported types | Use a supported type, or omit the type to generate a string |
| BTBP1007 | ⚠️ Warning | Value of a non-string const is empty | Give the entry a value, or declare it as a string |
| BTBP1008 | ⚠️ Warning | Value cannot be converted to the declared type | Fix the value so that it parses as the declared type |
| BTBP1009 | ⚠️ Warning | Const name appears more than once | Remove the duplicate entry |
