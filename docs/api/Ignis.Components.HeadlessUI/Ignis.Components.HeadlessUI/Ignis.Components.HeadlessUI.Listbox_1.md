[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# Listbox&lt;T&gt; Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

**Renders a listbox which can be used to select one or more values.**

```csharp
public sealed class Listbox<T> : Ignis.Components.HeadlessUI.OpenCloseWithTransitionComponentBase, IDynamicParentComponent<Ignis.Components.HeadlessUI.Listbox<T>>, Ignis.Components.IDynamicComponent, Ignis.Components.IElementReferenceProvider, IAriaPopup<ListboxOption<T>>, Ignis.Components.HeadlessUI.Aria.IAriaPopup, Ignis.Components.HeadlessUI.Aria.IAriaControl, Ignis.Components.HeadlessUI.Aria.IAriaComponent, Ignis.Components.HeadlessUI.Aria.IAriaComponentPart, Ignis.Components.HeadlessUI.Aria.IAriaLabeled
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [FocusComponentBase](../../Ignis.Components.Web/Ignis.Components.Web/Ignis.Components.Web.FocusComponentBase.md) → [OpenCloseWithTransitionComponentBase](Ignis.Components.HeadlessUI.OpenCloseWithTransitionComponentBase.md) → Listbox&lt;T&gt;

Implements: [IDynamicParentComponent&lt;Listbox&lt;T&gt;&gt;](../../Ignis.Components/Ignis.Components/Ignis.Components.IDynamicParentComponent{Ignis.Components.HeadlessUI.Listbox_1}.md), [IDynamicComponent](../../Ignis.Components/Ignis.Components/Ignis.Components.IDynamicComponent.md), [IElementReferenceProvider](../../Ignis.Components/Ignis.Components/Ignis.Components.IElementReferenceProvider.md), [IAriaPopup&lt;ListboxOption&lt;T&gt;&gt;](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaPopup{Ignis.Components.HeadlessUI.ListboxOption_1}.md), [IAriaPopup](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaPopup.md), [IAriaControl](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaControl.md), [IAriaComponent](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponent.md), [IAriaComponentPart](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponentPart.md), [IAriaLabeled](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaLabeled.md)

## Type Parameters

- `T`

  The value type.

## Constructors

|           | Summary                                                                                               |
| --------- | ----------------------------------------------------------------------------------------------------- |
| Listbox() | Initializes a new instance of the [Listbox&lt;T&gt;](Ignis.Components.HeadlessUI.Listbox_1.md) class. |

## Properties

|                      | Summary                                                                      |
| -------------------- | ---------------------------------------------------------------------------- |
| AsElement            |                                                                              |
| AsComponent          |                                                                              |
| Value                | Gets or sets the selected value.                                             |
| Values               | Gets or sets the selected values.                                            |
| ValueChanged         | Gets or sets the callback which is invoked when the selected value changes.  |
| ValuesChanged        | Gets or sets the callback which is invoked when the selected values changes. |
| \_                   |                                                                              |
| ChildContent         | Gets or sets the content of the listbox.                                     |
| AdditionalAttributes | Gets or sets additional attributes that will be applied to the listbox.      |
| Element              |                                                                              |
| Component            |                                                                              |
| Attributes           |                                                                              |
| Id                   |                                                                              |
| Descendants          |                                                                              |
| ActiveDescendant     |                                                                              |
| Controlled           |                                                                              |
| Button               |                                                                              |
| Label                |                                                                              |
| Targets              |                                                                              |
| KeysToCapture        |                                                                              |
| Multiple             |                                                                              |

## Methods

|                                                                              | Summary |
| ---------------------------------------------------------------------------- | ------- |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| AddDescendant(ListboxOption&lt;T&gt;)                                        |         |
| RemoveDescendant(ListboxOption&lt;T&gt;)                                     |         |
| GetId(Ignis.Components.HeadlessUI.Aria.IAriaComponentPart)                   |         |
| OnTargetBlur()                                                               |         |
| IsValueSelected(T)                                                           |         |
| SelectValue(T)                                                               |         |
| OnAfterOpen(System.Action)                                                   |         |
| OnKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs)             |         |
