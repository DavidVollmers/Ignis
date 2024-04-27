[Packages](../../README.md) / [Ignis.Fragments.Abstractions](../README.md) / [Ignis.Fragments.Abstractions](README.md) /

# AttributeCollection Class

## Definition

Namespace: [Ignis.Fragments.Abstractions](README.md)

Assembly: [Ignis.Fragments.Abstractions.dll](../README.md)

Package: [Ignis.Fragments.Abstractions](https://www.nuget.org/packages/Ignis.Fragments.Abstractions)

---

```csharp
public sealed class AttributeCollection : System.Collections.Generic.IReadOnlyDictionary<System.String, System.Object>, System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.KeyValuePair<System.String, System.Object>>, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<System.String, System.Object>>, System.Collections.IEnumerable
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → AttributeCollection

Implements: [System.Collections.Generic.IReadOnlyDictionary&lt;System.String, System.Object&gt;](https://learn.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IReadOnlyDictionary&lt;System.String, System.Object&gt;), [System.Collections.Generic.IReadOnlyCollection&lt;System.Collections.Generic.KeyValuePair&lt;System.String, System.Object&gt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IReadOnlyCollection&lt;System.Collections.Generic.KeyValuePair&lt;System.String, System.Object&gt;&gt;), [System.Collections.Generic.IEnumerable&lt;System.Collections.Generic.KeyValuePair&lt;System.String, System.Object&gt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/System.Collections.Generic.IEnumerable&lt;System.Collections.Generic.KeyValuePair&lt;System.String, System.Object&gt;&gt;), [System.Collections.IEnumerable](https://learn.microsoft.com/en-us/dotnet/api/System.Collections.IEnumerable)

## Constructors

|                                                                                                                                                | Summary |
| ---------------------------------------------------------------------------------------------------------------------------------------------- | ------- |
| AttributeCollection(System.Collections.Generic.IEnumerable&lt;System.Collections.Generic.KeyValuePair&lt;System.String, System.Object&gt;&gt;) |         |
| AttributeCollection()                                                                                                                          |         |

## Properties

|        | Summary |
| ------ | ------- |
| Item   |         |
| Count  |         |
| Keys   |         |
| Values |         |

## Methods

|                                            | Summary |
| ------------------------------------------ | ------- |
| ContainsKey(System.String)                 |         |
| TryGetValue(System.String, System.Object&) |         |
| GetEnumerator()                            |         |
