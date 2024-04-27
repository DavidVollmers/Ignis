[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# RadioGroup&lt;T&gt; Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

```csharp
public sealed class RadioGroup<T> : DynamicComponentBase<Ignis.Components.HeadlessUI.RadioGroup<T>>, Ignis.Components.HeadlessUI.Aria.IAriaCheckGroup, Ignis.Components.HeadlessUI.Aria.IAriaComponent, Ignis.Components.HeadlessUI.Aria.IAriaComponentPart, Ignis.Components.HeadlessUI.Aria.IAriaLabeled
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [DynamicComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.DynamicComponentBase_1.md)&lt;[RadioGroup&lt;T&gt;](Ignis.Components.HeadlessUI.RadioGroup_1.md)&gt; → RadioGroup&lt;T&gt;

Implements: [IAriaCheckGroup](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaCheckGroup.md), [IAriaComponent](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponent.md), [IAriaComponentPart](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaComponentPart.md), [IAriaLabeled](../Ignis.Components.HeadlessUI.Aria/Ignis.Components.HeadlessUI.Aria.IAriaLabeled.md)

## Type Parameters

- `T`

## Constructors

|              | Summary |
| ------------ | ------- |
| RadioGroup() |         |

## Properties

|              | Summary                                |
| ------------ | -------------------------------------- |
| Value        | The checked value.                     |
| ValueChanged | Occurs when the checked value changes. |
| ChildContent |                                        |
| Options      |                                        |
| ActiveOption |                                        |
| Label        |                                        |
| Id           |                                        |

## Methods

|                                                                              | Summary |
| ---------------------------------------------------------------------------- | ------- |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| GetId(Ignis.Components.HeadlessUI.Aria.IAriaComponentPart)                   |         |
| IsValueChecked&lt;T1&gt;(T1)                                                 |         |
| CheckValue&lt;T1&gt;(T1)                                                     |         |
| AddOption(RadioGroupOption&lt;T&gt;)                                         |         |
| RemoveOption(RadioGroupOption&lt;T&gt;)                                      |         |
