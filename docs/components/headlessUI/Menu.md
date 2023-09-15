---
order: 201
title: Menu (Dropdown)
category: HeadlessUI
permalink: /components/headlessUI/menu
inject:
  type: Ignis.Website.Examples.HeadlessUI.MenuExample
  description: Menus offer an easy way to build custom, accessible dropdown components with robust support for keyboard navigation.
api:
  - Ignis.Components.HeadlessUI.Menu, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.MenuButton, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.MenuItems, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.MenuItem, Ignis.Components.HeadlessUI
---

## Basic usage

Menu Buttons are built using the `Menu`, `MenuButton`, `MenuItems`, and `MenuItem` components.

The `MenuButton` will automatically open/close the `MenuItems` when clicked, and when the menu is open, the list of
items receives focus and is automatically navigable via the keyboard.

## Showing/hiding the menu

By default, your `MenuItems` instance will be shown/hidden automatically based on the internal `IsOpen` state
tracked within the `Menu` component itself.

## Transitions

To animate the opening/closing of the menu panel, use the provided `Transition` component. All you need to do is wrap
the `MenuItems` in a `Transition`, and the transition will be applied automatically.

You can read more about transitions [here](/docs/components/headlessUI/transition).

## Rendering a different element for a component

By default, the `Menu` and its subcomponents each render a default element that is sensible for that component.

For example, `MenuButton` renders a `button` by default, and `MenuItems` renders a `div`. By contrast, `Menu` and
`MenuItem` do not render an element, and instead render their children directly by default.

This is easy to change using the `AsElement` or `AsComponent` prop, which exists on every component.
You can read more about this [here](/docs/components/dynamic).

## Accessibility notes

### Focus management

Clicking the `MenuButton` toggles the menu and focuses the `MenuItems` component. Focus is trapped within the open menu
until <kbd>Escape</kbd> is pressed or the user clicks outside the menu. Closing the menu returns focus to
the `MenuButton`.

### Mouse interaction

Clicking a `MenuButton` toggles the menu. Clicking anywhere outside of an open menu will close that menu.

### Keyboard interaction

| Command                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             | Description                            |
| --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | -------------------------------------- |
| <kbd>Enter</kbd> <kbd>Space</kbd> <kbd><span class="sr-only">ArrowDown</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M4.095 3.578h2.808v5.28H8.38L5.5 12.422 2.62 8.858h1.476v-5.28z"></path></svg></kbd> <kbd><span class="sr-only">ArrowUp</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M6.903 12.422H4.095v-5.28H2.62L5.5 3.578l2.88 3.564H6.903v5.28z"></path></svg></kbd> when `MenuButton` is focused. | Opens menu and focuses the first item. |
| <kbd>Escape</kbd> when menu is open.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                | Closes menu.                           |
| <kbd><span class="sr-only">ArrowDown</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M4.095 3.578h2.808v5.28H8.38L5.5 12.422 2.62 8.858h1.476v-5.28z"></path></svg></kbd> <kbd><span class="sr-only">ArrowUp</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M6.903 12.422H4.095v-5.28H2.62L5.5 3.578l2.88 3.564H6.903v5.28z"></path></svg></kbd> when menu is open.                                              | Focuses previous/next item.            |
| <kbd>Enter</kbd> <kbd>Space</kbd> when menu is open.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                | Clicks the current item.               |

### Other

All relevant ARIA attributes are automatically managed.

## When to use a Menu

Menus are best for UI elements that resemble things like the menus you'd find in the title bar of most operating
systems. They have specific accessibility semantics, and their content should be restricted to a list of links or
buttons. Focus is trapped in an open menu, so you cannot Tab through the content or away from the menu. Instead, the
arrow keys navigate through a Menu's items.

Here's when you might use other similar components from Headless UI:

- `Popover`. Popovers are general-purpose floating menus. They appear near the button that triggers them, and you can
  put arbitrary markup in them like images or non-clickable content. The Tab key navigates the contents of a Popover
  like it would any other normal markup. They're great for building header nav items with expandable content and flyout
  panels.

- `Disclosure`. Disclosures are useful for elements that expand to reveal additional information, like a toggleable FAQ
  section. They are typically rendered inline and reflow the document when they're shown or hidden.

- `Dialog`. Dialogs are meant to grab the user's full attention. They typically render a floating panel in the center of
  the screen, and use a backdrop to dim the rest of the application's contents. They also capture focus and prevent
  tabbing away from the Dialog's contents until the Dialog is dismissed.
