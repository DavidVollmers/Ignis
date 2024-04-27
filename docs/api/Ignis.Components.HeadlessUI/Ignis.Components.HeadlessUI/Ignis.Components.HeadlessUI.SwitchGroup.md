[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# SwitchGroup Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

```csharp
public sealed class SwitchGroup : Ignis.Components.DynamicComponentBase<Ignis.Components.HeadlessUI.SwitchGroup>, Ignis.Components.HeadlessUI.Aria.IAriaCheckGroup, Ignis.Components.HeadlessUI.Aria.IAriaComponent, Ignis.Components.HeadlessUI.Aria.IAriaComponentPart, Ignis.Components.HeadlessUI.Aria.IAriaLabeled
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [DynamicComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.DynamicComponentBase_1.md)&lt;[SwitchGroup](Ignis.Components.HeadlessUI.SwitchGroup.md)&gt; → SwitchGroup

Implements: [IAriaCheckGroup](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaCheckGroup.md), [IAriaComponent](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponent.md), [IAriaComponentPart](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponentPart.md), [IAriaLabeled](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaLabeled.md)

## Constructors

|               | Summary |
| ------------- | ------- |
| SwitchGroup() |         |

## Properties

|              | Summary |
| ------------ | ------- |
| ChildContent |         |
| Label        |         |
| Switch       |         |
| Description  |         |
| Id           |         |

## Methods

|                                                                              | Summary |
| ---------------------------------------------------------------------------- | ------- |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| GetId(Ignis.Components.HeadlessUI.Aria.IAriaComponentPart)                   |         |
| ToggleSwitch()                                                               |         |
