# Diagnostics

## Class

| ID | Severity | Description | How to fix |
|---|---|---|---|
| BTBP0001 | ❌ Error | Generated namespace (`RootNamespace`) is not a valid namespace | Give `RootNamespace` a valid namespace |
| BTBP0002 | ❌ Error | Generated class name is not a valid identifier | Give `EmbeddedPropertyClass` a valid identifier |

## Entry

| ID | Severity | Description | How to fix |
|---|---|---|---|
| BTBP0101 | ⚠️ Warning | Entry has no `=` separating the name from the type | Write the entry as `Name=Type:Value` |
| BTBP0102 | ⚠️ Warning | Entry has no `:` separating the type from the value | Write the entry as `Name=Type:Value` |
| BTBP0103 | ⚠️ Warning | Const name is not a valid identifier | Rename the entry to a valid C# identifier |
| BTBP0104 | ⚠️ Warning | Const type is not one of the supported types | Use a supported type (`string`, `bool`, `char`, integer types, `float`, `double`, `decimal`) |
| BTBP0105 | ⚠️ Warning | Value of a non-string const is empty | Give the entry a value, or declare it as a string |
| BTBP0106 | ⚠️ Warning | Value cannot be converted to the declared type | Fix the value so that it parses as the declared type |
| BTBP0107 | ⚠️ Warning | Const name appears more than once | Remove the duplicate entry |
