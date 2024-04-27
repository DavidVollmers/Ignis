[Packages](../../README.md) / [Ignis.Components](../README.md) / [Ignis.Components](README.md) /

# IgnisAsyncComponentBase Class

## Definition

Namespace: [Ignis.Components](README.md)

Assembly: [Ignis.Components.dll](../README.md)

Package: [Ignis.Components](https://www.nuget.org/packages/Ignis.Components)

---

```csharp
public abstract class IgnisAsyncComponentBase : Ignis.Components.IgnisComponentBase, System.IDisposable
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](Ignis.Components.IgnisComponentBase.md) → IgnisAsyncComponentBase

Derived: [ScrollDetector](../../Ignis.Components.Web/Ignis.Components.Web/Ignis.Components.Web.ScrollDetector.md)

Implements: [System.IDisposable](https://learn.microsoft.com/en-us/dotnet/api/System.IDisposable)

## Constructors

|                           | Summary |
| ------------------------- | ------- |
| IgnisAsyncComponentBase() |         |

## Properties

|                   | Summary |
| ----------------- | ------- |
| CancellationToken |         |

## Methods

|                                                        | Summary |
| ------------------------------------------------------ | ------- |
| OnInitializedAsync(System.Threading.CancellationToken) |         |
| OnUpdateAsync(System.Threading.CancellationToken)      |         |
| Dispose(System.Boolean)                                |         |
| Dispose()                                              |         |
