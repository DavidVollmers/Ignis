[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# RadioGroupOption&lt;T&gt; Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

```csharp
public sealed class RadioGroupOption<T> : Ignis.Components.Web.FocusComponentBase, IDynamicParentComponent<Ignis.Components.HeadlessUI.RadioGroupOption<T>>, Ignis.Components.IDynamicComponent, Ignis.Components.IElementReferenceProvider, Ignis.Components.HeadlessUI.Aria.IAriaCheckGroupOption, Ignis.Components.HeadlessUI.Aria.IAriaLabeled, Ignis.Components.HeadlessUI.Aria.IAriaComponentPart, Ignis.Components.HeadlessUI.Aria.IAriaDescribed
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [FocusComponentBase](../../Ignis.Components.Web/Ignis.Components.Web/Ignis.Components.Web.FocusComponentBase.md) → RadioGroupOption&lt;T&gt;

Implements: [IDynamicParentComponent&lt;RadioGroupOption&lt;T&gt;&gt;](../../Ignis.Components/Ignis.Components/Ignis.Components.IDynamicParentComponent{Ignis.Components.HeadlessUI.RadioGroupOption_1}.md), [IDynamicComponent](../../Ignis.Components/Ignis.Components/Ignis.Components.IDynamicComponent.md), [IElementReferenceProvider](../../Ignis.Components/Ignis.Components/Ignis.Components.IElementReferenceProvider.md), [IAriaCheckGroupOption](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaCheckGroupOption.md), [IAriaLabeled](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaLabeled.md), [IAriaComponentPart](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponentPart.md), [IAriaDescribed](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaDescribed.md)

## Type Parameters

- `T`

## Constructors

|                    | Summary |
| ------------------ | ------- |
| RadioGroupOption() |         |

## Properties

|                      | Summary |
| -------------------- | ------- |
| Targets              |         |
| KeysToCapture        |         |
| Id                   |         |
| AsElement            |         |
| AsComponent          |         |
| Value                |         |
| OnClick              |         |
| RadioGroup           |         |
| \_                   |         |
| ChildContent         |         |
| AdditionalAttributes |         |
| IsActive             |         |
| IsChecked            |         |
| Label                |         |
| Description          |         |
| Element              |         |
| Component            |         |
| Attributes           |         |

## Methods

|                                                                              | Summary |
| ---------------------------------------------------------------------------- | ------- |
| OnInitialized()                                                              |         |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| OnKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs)             |         |
| Check()                                                                      |         |
| OnTargetFocus()                                                              |         |
| OnTargetBlur()                                                               |         |
| Dispose(System.Boolean)                                                      |         |
