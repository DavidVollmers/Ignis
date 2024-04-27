[Packages](../../README.md) / [Ignis.Components.Web](../README.md) / [Ignis.Components.Web](README.md) /

# FocusComponentBase Class

## Definition

Namespace: [Ignis.Components.Web](README.md)

Assembly: [Ignis.Components.Web.dll](../README.md)

Package: [Ignis.Components.Web](https://www.nuget.org/packages/Ignis.Components.Web)

---

```csharp
public abstract class FocusComponentBase : Ignis.Components.IgnisComponentBase, Ignis.Components.Web.IFocus, Microsoft.AspNetCore.Components.IHandleAfterRender, System.IDisposable
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → FocusComponentBase

Derived: [DialogPanel](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.DialogPanel.md), [OpenCloseWithTransitionComponentBase](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.OpenCloseWithTransitionComponentBase.md), [RadioGroupOption&lt;T&gt;](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.RadioGroupOption_1.md), [Tab](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.Tab.md)

Implements: [IFocus](Ignis.Components.Web.IFocus.md), [Microsoft.AspNetCore.Components.IHandleAfterRender](https://learn.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Components.IHandleAfterRender), [System.IDisposable](https://learn.microsoft.com/en-us/dotnet/api/System.IDisposable)

## Constructors

|                      | Summary |
| -------------------- | ------- |
| FocusComponentBase() |         |

## Properties

|               | Summary |
| ------------- | ------- |
| Targets       |         |
| KeysToCapture |         |
| FocusOnRender |         |
| OnFocus       |         |
| OnBlur        |         |
| JSRuntime     |         |

## Methods

|                                                                           | Summary                |
| ------------------------------------------------------------------------- | ---------------------- |
| InvokeFocusAsync()                                                        | For internal use only. |
| InvokeBlurAsync()                                                         | For internal use only. |
| InvokeKeyDownAsync(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs) | For internal use only. |
| FocusAsync()                                                              |                        |
| OnAfterRenderAsync()                                                      |                        |
| UpdateTargetsAsync()                                                      |                        |
| OnTargetFocus()                                                           |                        |
| OnTargetFocusAsync()                                                      |                        |
| OnTargetBlur()                                                            |                        |
| OnTargetBlurAsync()                                                       |                        |
| OnKeyDown(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs)          |                        |
| OnKeyDownAsync(Microsoft.AspNetCore.Components.Web.KeyboardEventArgs)     |                        |
| Dispose(System.Boolean)                                                   |                        |
| Dispose()                                                                 |                        |
