[Packages](../../README.md) / [Ignis.Components](../README.md) / [Ignis.Components](README.md) /

# IgnisComponentBase Class

## Definition

Namespace: [Ignis.Components](README.md)

Assembly: [Ignis.Components.dll](../README.md)

Package: [Ignis.Components](https://www.nuget.org/packages/Ignis.Components)

---

```csharp
public abstract class IgnisComponentBase : Microsoft.AspNetCore.Components.IComponent
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → IgnisComponentBase

Derived: [ContentHostBase](Ignis.Components.ContentHostBase.md), [ContentProviderBase](Ignis.Components.ContentProviderBase.md), [DynamicComponentBase&lt;T&gt;](Ignis.Components.DynamicComponentBase_1.md), [Fragment](Ignis.Components.Fragment.md), [IgnisAsyncComponentBase](Ignis.Components.IgnisAsyncComponentBase.md), [ReactiveSection](../../Ignis.Components.Reactivity/Ignis.Components.Reactivity/Ignis.Components.Reactivity.ReactiveSection.md), [FocusComponentBase](../../Ignis.Components.Web/Ignis.Components.Web/Ignis.Components.Web.FocusComponentBase.md), [HeroIconBase](../../Ignis.Components.HeroIcons/Ignis.Components.HeroIcons/Ignis.Components.HeroIcons.HeroIconBase.md)

Implements: [Microsoft.AspNetCore.Components.IComponent](https://learn.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Components.IComponent)

## Constructors

|                      | Summary |
| -------------------- | ------- |
| IgnisComponentBase() |         |

## Properties

|              | Summary |
| ------------ | ------- |
| ShouldRender |         |
| HostContext  |         |

## Methods

|                                                                              | Summary |
| ---------------------------------------------------------------------------- | ------- |
| Attach(Microsoft.AspNetCore.Components.RenderHandle)                         |         |
| SetParametersAsync(Microsoft.AspNetCore.Components.ParameterView)            |         |
| BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder) |         |
| OnInitialized()                                                              |         |
| OnUpdate()                                                                   |         |
