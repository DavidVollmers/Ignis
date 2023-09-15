---
order: 205
title: Disclosure
category: HeadlessUI
permalink: /components/headlessUI/disclosure
inject:
  type: Ignis.Website.Examples.HeadlessUI.DisclosureExample
  description: A simple, accessible foundation for building custom UIs that show and hide content, like togglable accordion panels.
api:
  - Ignis.Components.HeadlessUI.Disclosure, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.DisclosureButton, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.DisclosurePanel, Ignis.Components.HeadlessUI
---

## Basic usage

Disclosures are built using the `Disclosure`, `DisclosureButton` and `DisclosurePanel` components.

The button will automatically open/close the panel when clicked, and all components will receive the appropriate aria-\*
related attributes like `aria-expanded` and `aria-controls`.

## Closing disclosures manually

To close a disclosure manually when clicking a child of its panel, render that child as a `DisclosureButton`. You can
use the `AsElement` or `AsComponent` prop to customize which element is being rendered.

This is especially useful when using disclosures for things like mobile menus that contain links where you want the
disclosure to close when navigating to the next page.

Alternatively, `Disclosure` expose a `Close()` method which you can use to imperatively close the panel, say after
running an async action.

## Transitions

To animate the opening/closing of the menu panel, use the provided `Transition` component. All you need to do is wrap
the `DisclosurePanel` in a `Transition`, and the transition will be applied automatically.

You can read more about transitions [here](/docs/components/headlessUI/transition).

## Rendering a different element for a component

`Disclosure` and its subcomponents each render a default element that is sensible for that component:
All `DisclosureButton` renders a `button`, `DisclosurePanel` renders a `div`. By contrast, the root `Disclosure`
component does not render an element, and instead renders its children directly by default.

This is easy to change using the `AsElement` or `AsComponent` prop, which exists on every component.
You can read more about this [here](/docs/components/dynamic).

## Accessibility notes

### Mouse interaction

Clicking a `DisclosureButton` toggles the Disclosure's panel open and closed.

### Keyboard interaction

| Command                                                                 | Description    |
| ----------------------------------------------------------------------- | -------------- |
| <kbd>Enter</kbd> <kbd>Space</kbd> when a `DisclosureButton` is focused. | Toggles panel. |

### Other

All relevant ARIA attributes are automatically managed.
