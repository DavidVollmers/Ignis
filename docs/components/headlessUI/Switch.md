---
order: 204
title: Switch (Toggle)
category: HeadlessUI
permalink: /components/headlessUI/switch
inject:
  type: Ignis.Website.Examples.HeadlessUI.SwitchExample
  description: Switches are a pleasant interface for toggling a value between two states, and offer the same semantics and keyboard navigation as native checkbox elements.
api:
  - Ignis.Components.HeadlessUI.Switch, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.SwitchLabel, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.SwitchDescription, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.SwitchGroup, Ignis.Components.HeadlessUI
---

## Basic usage

Switches are built using the `Switch` component. You can toggle your Switch by clicking directly on the component, or by
pressing the spacebar while its focused.

## Using a custom label

By default, a `Switch` renders a `button` as well as whatever children you pass into it. This can make it harder to
implement certain UIs, since the children will be nested within the button.

In these situations, you can use the `SwitchLabel` component for more flexibility.

This example demonstrates how to use the `SwitchGroup`, `Switch` and `SwitchLabel` components to render a label as a
sibling to the button. Note that `SwitchLabel` works alongside a `Switch` component, and they both must be rendered
within a parent `SwitchGroup` component.

## Transitions

Because switches are typically always rendered to the DOM (rather than being mounted/unmounted like other components),
simple CSS transitions are often enough to animate your Switch.

## Accessibility notes

### Labels

By default, the children of a `Switch` will be used as the label for screen readers. If you're using `SwitchLabel`, the
content of your `Switch` component will be ignored by assistive technologies.

### Mouse interaction

Clicking a `Switch` or a `SwitchLabel` toggles the Switch on and off.

### Keyboard interaction

| Command                                      | Description         |
| -------------------------------------------- | ------------------- |
| <kbd>Space</kbd> when a `Switch` is focused. | Toggles the switch. |

### Other

All relevant ARIA attributes are automatically managed.
