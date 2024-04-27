[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI](README.md) /

# TransitionChild Class

## Definition

Namespace: [Ignis.Components.HeadlessUI](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

```csharp
public sealed class TransitionChild : Ignis.Components.HeadlessUI.TransitionBase<Ignis.Components.HeadlessUI.TransitionChild>, System.IDisposable
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.IgnisComponentBase.md) → [DynamicComponentBase](../../Ignis.Components/Ignis.Components/Ignis.Components.DynamicComponentBase_1.md)&lt;[TransitionChild](Ignis.Components.HeadlessUI.TransitionChild.md)&gt; → [TransitionBase](Ignis.Components.HeadlessUI.TransitionBase_1.md)&lt;[TransitionChild](Ignis.Components.HeadlessUI.TransitionChild.md)&gt; → TransitionChild

Implements: [System.IDisposable](https://learn.microsoft.com/en-us/dotnet/api/System.IDisposable)

## Constructors

|                   | Summary |
| ----------------- | ------- |
| TransitionChild() |         |

## Properties

|              | Summary |
| ------------ | ------- |
| Parent       |         |
| ChildContent |         |

## Methods

|                                                                              | Summary |
| ---------------------------------------------------------------------------- | ------- |
| OnInitialized()                                                              |         |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| Hide(System.Action)                                                          |         |
| Show(System.Action)                                                          |         |
| Dispose()                                                                    |         |
