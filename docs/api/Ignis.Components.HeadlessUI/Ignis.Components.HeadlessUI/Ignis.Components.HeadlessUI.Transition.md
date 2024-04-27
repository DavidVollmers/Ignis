[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# Transition Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

```csharp
public sealed class Transition : Ignis.Components.HeadlessUI.TransitionBase<Ignis.Components.HeadlessUI.Transition>, Ignis.Components.IContentProvider, System.IDisposable
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [DynamicComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.DynamicComponentBase_1.md)&lt;[Transition](Ignis.Components.HeadlessUI.Transition.md)&gt; → [TransitionBase](Ignis.Components.HeadlessUI.TransitionBase_1.md)&lt;[Transition](Ignis.Components.HeadlessUI.Transition.md)&gt; → Transition

Implements: [IContentProvider](../../Ignis.Components/Ignis.Components/Ignis.Components.IContentProvider.md), [System.IDisposable](https://learn.microsoft.com/en-us/dotnet/api/System.IDisposable)

## Constructors

|              | Summary |
| ------------ | ------- |
| Transition() |         |

## Properties

|                  | Summary |
| ---------------- | ------- |
| Show             |         |
| Appear           |         |
| Outlet           |         |
| Menu             |         |
| Listbox          |         |
| Popover          |         |
| Disclosure       |         |
| ChildContent     |         |
| Content          |         |
| HasOutletDialogs |         |
| ContentRegistry  |         |

## Methods

|                                                                              | Summary |
| ---------------------------------------------------------------------------- | ------- |
| OnInitialized()                                                              |         |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| HostedBy(Ignis.Components.IContentHost)                                      |         |
| EnterTransition(System.Action)                                               |         |
| LeaveTransition(System.Action)                                               |         |
| AddChild(Ignis.Components.HeadlessUI.TransitionChild)                        |         |
| RemoveChild(Ignis.Components.HeadlessUI.TransitionChild)                     |         |
| AddDialog(Ignis.Components.HeadlessUI.Dialog)                                |         |
| RemoveDialog(Ignis.Components.HeadlessUI.Dialog)                             |         |
| OnAfterRenderAsync()                                                         |         |
| Dispose()                                                                    |         |
