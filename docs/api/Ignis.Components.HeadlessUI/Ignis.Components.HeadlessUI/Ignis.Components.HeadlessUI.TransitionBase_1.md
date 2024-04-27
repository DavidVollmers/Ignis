[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# TransitionBase&lt;T&gt; Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

```csharp
public abstract class TransitionBase<T> : DynamicComponentBase<T>, Ignis.Components.Web.ICssClass, Microsoft.AspNetCore.Components.IHandleAfterRender
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [DynamicComponentBase&lt;T&gt;](../../Ignis.Components/Ignis.Components/Ignis.Components.DynamicComponentBase_1.md) → TransitionBase&lt;T&gt;

Derived: [Transition](Ignis.Components.HeadlessUI.Transition.md), [TransitionChild](Ignis.Components.HeadlessUI.TransitionChild.md)

Implements: [ICssClass](../../Ignis.Components.Web/Ignis.Components.Web/Ignis.Components.Web.ICssClass.md), [Microsoft.AspNetCore.Components.IHandleAfterRender](https://learn.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Components.IHandleAfterRender)

## Type Parameters

- `T`

## Constructors

|                               | Summary |
| ----------------------------- | ------- |
| TransitionBase(System.String) |         |
| TransitionBase(System.Type)   |         |

## Properties

|               | Summary |
| ------------- | ------- |
| RenderContent |         |
| Enter         |         |
| EnterFrom     |         |
| EnterTo       |         |
| Leave         |         |
| LeaveFrom     |         |
| LeaveTo       |         |
| CssClass      |         |
| Attributes    |         |

## Methods

|                                | Summary |
| ------------------------------ | ------- |
| EnterTransition(System.Action) |         |
| LeaveTransition(System.Action) |         |
| OnAfterRenderAsync()           |         |
