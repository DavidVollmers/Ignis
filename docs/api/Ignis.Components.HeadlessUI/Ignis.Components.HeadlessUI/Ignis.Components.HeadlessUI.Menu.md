[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# Menu Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

```csharp
public sealed class Menu : Ignis.Components.HeadlessUI.OpenCloseWithTransitionComponentBase, Ignis.Components.IDynamicParentComponent<Ignis.Components.HeadlessUI.Menu>, Ignis.Components.IDynamicComponent, Ignis.Components.IElementReferenceProvider, Ignis.Components.HeadlessUI.Aria.IAriaPopup<Ignis.Components.HeadlessUI.MenuItem>, Ignis.Components.HeadlessUI.Aria.IAriaPopup, Ignis.Components.HeadlessUI.Aria.IAriaControl, Ignis.Components.HeadlessUI.Aria.IAriaComponent, Ignis.Components.HeadlessUI.Aria.IAriaComponentPart, Ignis.Components.HeadlessUI.Aria.IAriaLabeled
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [FocusComponentBase](../../Ignis.Components.Web/Ignis.Components.Web/Ignis.Components.Web.FocusComponentBase.md) → [OpenCloseWithTransitionComponentBase](Ignis.Components.HeadlessUI.OpenCloseWithTransitionComponentBase.md) → Menu

Implements: [IDynamicParentComponent&lt;Menu&gt;](../../Ignis.Components/Ignis.Components/Ignis.Components.IDynamicParentComponent{Ignis.Components.HeadlessUI.Menu}.md), [IDynamicComponent](../../Ignis.Components/Ignis.Components/Ignis.Components.IDynamicComponent.md), [IElementReferenceProvider](../../Ignis.Components/Ignis.Components/Ignis.Components.IElementReferenceProvider.md), [IAriaPopup&lt;MenuItem&gt;](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaPopup{Ignis.Components.HeadlessUI.MenuItem}.md), [IAriaPopup](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaPopup.md), [IAriaControl](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaControl.md), [IAriaComponent](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponent.md), [IAriaComponentPart](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponentPart.md), [IAriaLabeled](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaLabeled.md)

## Constructors

|        | Summary |
| ------ | ------- |
| Menu() |         |

## Properties

|                      | Summary                                          |
| -------------------- | ------------------------------------------------ |
| AsElement            |                                                  |
| AsComponent          |                                                  |
| \_                   |                                                  |
| ChildContent         |                                                  |
| AdditionalAttributes | Additional attributes to be applied to the menu. |
| Element              |                                                  |
| Component            |                                                  |
| Attributes           |                                                  |
| Id                   |                                                  |
| Descendants          |                                                  |
| ActiveDescendant     |                                                  |
| Controlled           |                                                  |
| Button               |                                                  |
| Label                |                                                  |
| Targets              |                                                  |
| KeysToCapture        |                                                  |

## Methods

|                                                                              | Summary |
| ---------------------------------------------------------------------------- | ------- |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| AddDescendant(Ignis.Components.HeadlessUI.MenuItem)                          |         |
| RemoveDescendant(Ignis.Components.HeadlessUI.MenuItem)                       |         |
| GetId(Ignis.Components.HeadlessUI.Aria.IAriaComponentPart)                   |         |
| OnTargetBlur()                                                               |         |
| OnAfterOpen(System.Action)                                                   |         |
| OnKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs)             |         |
