[Packages](../../README.md) / [Ignis.Components](../README.md) / [Ignis.Components](README.md) /

# DynamicComponentBase&lt;T&gt; Class

## Definition

Namespace: [Ignis.Components](README.md)

Assembly: [Ignis.Components.dll](../README.md)

Package: [Ignis.Components](https://www.nuget.org/packages/Ignis.Components)

---

```csharp
public abstract class DynamicComponentBase<T> : Ignis.Components.IgnisComponentBase, IDynamicParentComponent<T>, Ignis.Components.IDynamicComponent, Ignis.Components.IElementReferenceProvider
```

Inheritance: [System.Object](https://learn.microsoft.com/en-us/dotnet/api/System.Object) → [IgnisComponentBase](Ignis.Components.IgnisComponentBase.md) → DynamicComponentBase&lt;T&gt;

Derived: [Dynamic](Ignis.Components.Dynamic.md), [DialogDescription](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.DialogDescription.md), [DialogTitle](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.DialogTitle.md), [DisclosureButton](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.DisclosureButton.md), [DisclosurePanel](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.DisclosurePanel.md), [ListboxButton](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.ListboxButton.md), [ListboxLabel](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.ListboxLabel.md), [ListboxOptions](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.ListboxOptions.md), [MenuButton](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.MenuButton.md), [MenuItem](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.MenuItem.md), [MenuItems](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.MenuItems.md), [PopoverButton](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.PopoverButton.md), [PopoverPanel](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.PopoverPanel.md), [RadioGroupDescription](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.RadioGroupDescription.md), [RadioGroupLabel](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.RadioGroupLabel.md), [Switch](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.Switch.md), [SwitchDescription](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.SwitchDescription.md), [SwitchGroup](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.SwitchGroup.md), [SwitchLabel](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.SwitchLabel.md), [TabGroup](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.TabGroup.md), [TabList](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.TabList.md), [TabPanel](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.TabPanel.md), [TabPanels](../../Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI/Ignis.Components.HeadlessUI.TabPanels.md)

Implements: [IDynamicParentComponent&lt;T&gt;](Ignis.Components.IDynamicParentComponent{__0}.md), [IDynamicComponent](Ignis.Components.IDynamicComponent.md), [IElementReferenceProvider](Ignis.Components.IElementReferenceProvider.md)

## Type Parameters

- `T`

## Constructors

|                                     | Summary |
| ----------------------------------- | ------- |
| DynamicComponentBase(System.String) |         |
| DynamicComponentBase(System.Type)   |         |

## Properties

|                      | Summary |
| -------------------- | ------- |
| AsElement            |         |
| AsComponent          |         |
| \_                   |         |
| AdditionalAttributes |         |
| Element              |         |
| Component            |         |
| Attributes           |         |

## Methods

|                                                                                                                                                             | Summary |
| ----------------------------------------------------------------------------------------------------------------------------------------------------------- | ------- |
| SetAttributes(System.Collections.Generic.IEnumerable&lt;System.Func&lt;System.Collections.Generic.KeyValuePair&lt;System.String, System.Object&gt;&gt;&gt;) |         |
