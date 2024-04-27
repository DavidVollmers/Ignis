[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# MenuItem Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

```csharp
public sealed class MenuItem : Ignis.Components.DynamicComponentBase<Ignis.Components.HeadlessUI.MenuItem>, Ignis.Components.HeadlessUI.Aria.IAriaDescendant, Ignis.Components.HeadlessUI.Aria.IAriaComponentPart, System.IDisposable
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [DynamicComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.DynamicComponentBase_1.md)&lt;[MenuItem](Ignis.Components.HeadlessUI.MenuItem.md)&gt; → MenuItem

Implements: [IAriaDescendant](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaDescendant.md), [IAriaComponentPart](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponentPart.md), [System.IDisposable](https://learn.microsoft.com/en-us/dotnet/api/System.IDisposable)

## Constructors

|            | Summary |
| ---------- | ------- |
| MenuItem() |         |

## Properties

|              | Summary |
| ------------ | ------- |
| Id           |         |
| OnClick      |         |
| Menu         |         |
| ChildContent |         |
| IsActive     |         |

## Methods

|                                                                              | Summary |
| ---------------------------------------------------------------------------- | ------- |
| OnInitialized()                                                              |         |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| Click()                                                                      |         |
| Dispose()                                                                    |         |
