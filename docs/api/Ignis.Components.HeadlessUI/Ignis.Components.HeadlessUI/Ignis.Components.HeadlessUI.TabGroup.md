[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# TabGroup Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

```csharp
public sealed class TabGroup : Ignis.Components.DynamicComponentBase<Ignis.Components.HeadlessUI.TabGroup>, Ignis.Components.HeadlessUI.Aria.IAriaComponent, Ignis.Components.HeadlessUI.Aria.IAriaComponentPart
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [DynamicComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.DynamicComponentBase_1.md)&lt;[TabGroup](Ignis.Components.HeadlessUI.TabGroup.md)&gt; → TabGroup

Implements: [IAriaComponent](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponent.md), [IAriaComponentPart](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponentPart.md)

## Constructors

|            | Summary |
| ---------- | ------- |
| TabGroup() |         |

## Properties

|                      | Summary |
| -------------------- | ------- |
| Id                   |         |
| DefaultIndex         |         |
| SelectedIndex        |         |
| SelectedIndexChanged |         |
| ChildContent         |         |
| Tabs                 |         |
| TabPanels            |         |

## Methods

|                                                                              | Summary |
| ---------------------------------------------------------------------------- | ------- |
| OnInitialized()                                                              |         |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| GetId(Ignis.Components.HeadlessUI.Aria.IAriaComponentPart)                   |         |
| IsTabSelected(Ignis.Components.HeadlessUI.Tab)                               |         |
| SelectTab(Ignis.Components.HeadlessUI.Tab)                                   |         |
| AddTab(Ignis.Components.HeadlessUI.Tab)                                      |         |
| RemoveTab(Ignis.Components.HeadlessUI.Tab)                                   |         |
| IsTabPanelSelected(Ignis.Components.HeadlessUI.TabPanel)                     |         |
| AddTabPanel(Ignis.Components.HeadlessUI.TabPanel)                            |         |
| RemoveTabPanel(Ignis.Components.HeadlessUI.TabPanel)                         |         |
