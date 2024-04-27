[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# Popover Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

```csharp
public sealed class Popover : Ignis.Components.HeadlessUI.OpenCloseWithTransitionComponentBase, Ignis.Components.IDynamicParentComponent<Ignis.Components.HeadlessUI.Popover>, Ignis.Components.IDynamicComponent, Ignis.Components.IElementReferenceProvider, Ignis.Components.HeadlessUI.Aria.IAriaControl, Ignis.Components.HeadlessUI.Aria.IAriaComponent, Ignis.Components.HeadlessUI.Aria.IAriaComponentPart
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [FocusComponentBase](../../Ignis.Components.Web/Ignis.Components.Web/Ignis.Components.Web.FocusComponentBase.md) → [OpenCloseWithTransitionComponentBase](Ignis.Components.HeadlessUI.OpenCloseWithTransitionComponentBase.md) → Popover

Implements: [IDynamicParentComponent&lt;Popover&gt;](../../Ignis.Components/Ignis.Components/Ignis.Components.IDynamicParentComponent{Ignis.Components.HeadlessUI.Popover}.md), [IDynamicComponent](../../Ignis.Components/Ignis.Components/Ignis.Components.IDynamicComponent.md), [IElementReferenceProvider](../../Ignis.Components/Ignis.Components/Ignis.Components.IElementReferenceProvider.md), [IAriaControl](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaControl.md), [IAriaComponent](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponent.md), [IAriaComponentPart](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponentPart.md)

## Constructors

|           | Summary |
| --------- | ------- |
| Popover() |         |

## Properties

|                      | Summary                                             |
| -------------------- | --------------------------------------------------- |
| Targets              |                                                     |
| KeysToCapture        |                                                     |
| AsElement            |                                                     |
| AsComponent          |                                                     |
| \_                   |                                                     |
| ChildContent         |                                                     |
| AdditionalAttributes | Additional attributes to be applied to the popover. |
| Id                   |                                                     |
| Button               |                                                     |
| Controlled           |                                                     |
| Element              |                                                     |
| Component            |                                                     |
| Attributes           |                                                     |

## Methods

|                                                                              | Summary |
| ---------------------------------------------------------------------------- | ------- |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| OnTargetBlur()                                                               |         |
| OnKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs)             |         |
| GetId(Ignis.Components.HeadlessUI.Aria.IAriaComponentPart)                   |         |
