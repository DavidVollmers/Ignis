[Packages](../../README.md) / [Ignis.Components](../README.md) / [Ignis.Components](README.md) /

# ContentHostBase Class

## Definition

Namespace: [Ignis.Components](README.md)

Assembly: [Ignis.Components.dll](../README.md)

Package: [Ignis.Components](https://www.nuget.org/packages/Ignis.Components)

---

```csharp
public abstract class ContentHostBase : Ignis.Components.IgnisComponentBase, Ignis.Components.IContentHost, Ignis.Components.IContentRegistrySubscriber, System.IDisposable
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](Ignis.Components.IgnisComponentBase.md) → ContentHostBase

Derived: [DialogOutlet](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.DialogOutlet.md)

Implements: [IContentHost](Ignis.Components.IContentHost.md), [IContentRegistrySubscriber](Ignis.Components.IContentRegistrySubscriber.md), [System.IDisposable](https://learn.microsoft.com/en-us/dotnet/api/System.IDisposable)

## Constructors

|                   | Summary |
| ----------------- | ------- |
| ContentHostBase() |         |

## Properties

|                | Summary |
| -------------- | ------- |
| Components     |         |
| OutletRegistry |         |

## Methods

|                                                           | Summary |
| --------------------------------------------------------- | ------- |
| OnProviderRegistered(Ignis.Components.IContentProvider)   |         |
| OnProviderUnregistered(Ignis.Components.IContentProvider) |         |
| Dispose(System.Boolean)                                   |         |
| Dispose()                                                 |         |
