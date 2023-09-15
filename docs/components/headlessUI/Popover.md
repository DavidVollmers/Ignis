---
order: 207
title: Popover
category: HeadlessUI
permalink: /components/headlessUI/popover
inject:
  type: Ignis.Website.Examples.HeadlessUI.PopoverExample
  description: Popovers are perfect for floating panels with arbitrary content like navigation menus, mobile menus and flyout menus.
api:
  - Ignis.Components.HeadlessUI.Popover, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.PopoverButton, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.PopoverPanel, Ignis.Components.HeadlessUI
---

## Basic usage

Popovers are built using the `Popover`, `PopoverButton`, and `PopoverPanel` components.

Clicking the `PopoverButton` will automatically open/close the `PopoverPanel`. When the panel is open, clicking anywhere
outside of its contents, pressing the Escape key, or tabbing away from it will close the Popover.

## Showing/hiding the popover

By default, your `PopoverPanel` will be shown/hidden automatically based on the internal `IsOpen` state tracked within
the `Popover` component itself.

## Transitions

To animate the opening/closing of the popover panel, use the provided `Transition` component. All you need to do is wrap
the `PopoverPanel` in a `Transition`, and the transition will be applied automatically.

You can read more about transitions [here](/docs/components/headlessUI/transition).

## Rendering a different element for a component

`Popover` and its subcomponents each render a default element that is sensible for that component: the `Popover`
and `PopoverPanel` components render a `div`, and the `PopoverButton` component renders a `button`.

This is easy to change using the `AsElement` or `AsComponent` prop, which exists on every component.
You can read more about this [here](/docs/components/dynamic).

## Accessibility notes

### Mouse interaction

Clicking a `PopoverButton` toggles a panel open and closed. Clicking anywhere outside of an open panel will close that
panel.

### Keyboard interaction

| Command                                                              | Description               |
| -------------------------------------------------------------------- | ------------------------- |
| <kbd>Enter</kbd> <kbd>Space</kbd> when a `PopoverButton` is focused. | Toggle panel.             |
| <kbd>Escape</kbd>                                                    | Closes any open Popovers. |

### Other

All relevant ARIA attributes are automatically managed.

## When to use a Popover

Here's how Popovers compare to other similar components:

- `Menu`. Popovers are more general-purpose than Menus. Menus only support very restricted content and have specific
  accessibility semantics. Arrow keys also navigate a Menu's items. Menus are best for UI elements that resemble things
  like the menus you'd find in the title bar of most operating systems. If your floating panel has images or more markup
  than simple links, use a Popover.

- `Disclosure`. Disclosures are useful for things that typically reflow the document, like Accordions. Popovers also
  have extra behavior on top of Disclosures: they render overlays, and are closed when the user either clicks the
  overlay (by clicking outside of the Popover's content) or presses the escape key. If your UI element needs this
  behavior, use a Popover instead of a Disclosure.

- `Dialog`. Dialogs are meant to grab the user's full attention. They typically render a floating panel in the center of
  the screen, and use a backdrop to dim the rest of the application's contents. They also capture focus and prevent
  tabbing away from the Dialog's contents until the Dialog is dismissed. Popovers are more contextual, and are usually
  positioned near the element that triggered them.
