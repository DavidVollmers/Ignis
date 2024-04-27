[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI.Aria](README.md) /

# IAriaPopup&lt;T&gt; Interface

## Definition

Namespace: [Ignis.Components.HeadlessUI.Aria](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

**An ARIA conform popup.**

```csharp
public interface IAriaPopup<T> : Ignis.Components.HeadlessUI.Aria.IAriaPopup, Ignis.Components.HeadlessUI.Aria.IAriaControl, Ignis.Components.HeadlessUI.Aria.IAriaComponent, Ignis.Components.HeadlessUI.Aria.IAriaComponentPart, Ignis.Components.IElementReferenceProvider, Ignis.Components.HeadlessUI.Aria.IAriaLabeled, Ignis.Components.HeadlessUI.IOpenClose, Ignis.Components.HeadlessUI.IWithTransition, Ignis.Components.Web.IFocus
```

Implements: [IAriaPopup](Ignis.Components.HeadlessUI.Aria.IAriaPopup.md), [IAriaControl](Ignis.Components.HeadlessUI.Aria.IAriaControl.md), [IAriaComponent](Ignis.Components.HeadlessUI.Aria.IAriaComponent.md), [IAriaComponentPart](Ignis.Components.HeadlessUI.Aria.IAriaComponentPart.md), [IElementReferenceProvider](../../Ignis.Components/Ignis.Components/Ignis.Components.IElementReferenceProvider.md), [IAriaLabeled](Ignis.Components.HeadlessUI.Aria.IAriaLabeled.md), [IOpenClose](../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.IOpenClose.md), [IWithTransition](../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.IWithTransition.md), [IFocus](../../Ignis.Components.Web/Ignis.Components.Web/Ignis.Components.Web.IFocus.md)

## Type Parameters

- `T`

  The type of popup descendants.

## Properties

|                  | Summary |
| ---------------- | ------- |
| Descendants      |         |
| ActiveDescendant |         |

## Methods

|                     | Summary                                  |
| ------------------- | ---------------------------------------- |
| AddDescendant(T)    | Adds a descendant to the component.      |
| RemoveDescendant(T) | Removes a descendant from the component. |
