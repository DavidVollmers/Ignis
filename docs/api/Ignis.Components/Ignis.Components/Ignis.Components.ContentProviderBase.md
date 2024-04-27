[Packages](../../README.md) / [Ignis.Components](../README.md) / [Ignis.Components](README.md) /

# ContentProviderBase Class

## Definition

Namespace: [Ignis.Components](README.md)

Assembly: [Ignis.Components.dll](../README.md)

Package: [Ignis.Components](https://www.nuget.org/packages/Ignis.Components)

---

```csharp
public abstract class ContentProviderBase : Ignis.Components.IgnisComponentBase, Ignis.Components.IContentProvider, System.IDisposable
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](Ignis.Components.IgnisComponentBase.md) → ContentProviderBase

Derived: [Dialog](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.Dialog.md)

Implements: [IContentProvider](Ignis.Components.IContentProvider.md), [System.IDisposable](https://learn.microsoft.com/en-us/dotnet/api/System.IDisposable)

## Constructors

|                       | Summary |
| --------------------- | ------- |
| ContentProviderBase() |         |

## Properties

|                 | Summary |
| --------------- | ------- |
| IgnoreOutlet    |         |
| Outlet          |         |
| Content         |         |
| ContentRegistry |         |

## Methods

|                                                                                     | Summary |
| ----------------------------------------------------------------------------------- | ------- |
| OnInitialized()                                                                     |         |
| RegisterAsContentProvider()                                                         |         |
| HostedBy(Ignis.Components.IContentHost)                                             |         |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder)        |         |
| BuildContentRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| Dispose(System.Boolean)                                                             |         |
| Dispose()                                                                           |         |
