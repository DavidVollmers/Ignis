[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# OpenCloseWithTransitionComponentBase Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

```csharp
public abstract class OpenCloseWithTransitionComponentBase : Ignis.Components.Web.FocusComponentBase, Ignis.Components.HeadlessUI.IOpenClose, Ignis.Components.HeadlessUI.IWithTransition
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [FocusComponentBase](../../Ignis.Components.Web/Ignis.Components.Web/Ignis.Components.Web.FocusComponentBase.md) → OpenCloseWithTransitionComponentBase

Derived: [Disclosure](Ignis.Components.HeadlessUI.Disclosure.md), [Listbox&lt;T&gt;](Ignis.Components.HeadlessUI.Listbox_1.md), [Menu](Ignis.Components.HeadlessUI.Menu.md), [Popover](Ignis.Components.HeadlessUI.Popover.md)

Implements: [IOpenClose](Ignis.Components.HeadlessUI.IOpenClose.md), [IWithTransition](Ignis.Components.HeadlessUI.IWithTransition.md)

## Constructors

|                                        | Summary |
| -------------------------------------- | ------- |
| OpenCloseWithTransitionComponentBase() |         |

## Properties

|               | Summary |
| ------------- | ------- |
| IsOpen        |         |
| IsOpenChanged |         |
| Transition    |         |

## Methods

|                            | Summary |
| -------------------------- | ------- |
| Open(System.Action)        |         |
| OnAfterOpen(System.Action) |         |
| Close(System.Action)       |         |
| OnAfterRenderAsync()       |         |
