[Packages](../../README.md) / [Ignis.Components.HeadlessUI](../README.md) / [Ignis.Components.HeadlessUI.Aria](README.md) /

# IAriaControl Interface

## Definition

Namespace: [Ignis.Components.HeadlessUI.Aria](README.md)

Assembly: [Ignis.Components.HeadlessUI.dll](../README.md)

Package: [Ignis.Components.HeadlessUI](https://www.nuget.org/packages/Ignis.Components.HeadlessUI)

---

**A ARIA conform component that controls another component part.**

```csharp
public interface IAriaControl : Ignis.Components.HeadlessUI.Aria.IAriaComponent, Ignis.Components.HeadlessUI.Aria.IAriaComponentPart, Ignis.Components.IElementReferenceProvider
```

Implements: [IAriaComponent](Ignis.Components.HeadlessUI.Aria.IAriaComponent.md), [IAriaComponentPart](Ignis.Components.HeadlessUI.Aria.IAriaComponentPart.md), [IElementReferenceProvider](../../Ignis.Components/Ignis.Components/Ignis.Components.IElementReferenceProvider.md)

## Properties

|            | Summary                                                  |
| ---------- | -------------------------------------------------------- |
| Controlled | The component part that is controlled by this component. |
