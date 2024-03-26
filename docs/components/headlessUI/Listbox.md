---
order: 202
title: Listbox (Select)
category: HeadlessUI
permalink: /components/headlessUI/listbox
inject:
  type: Ignis.Website.Examples.HeadlessUI.ListboxExample
  description: Listboxes are a great foundation for building custom, accessible select menus for your app, complete with robust support for keyboard navigation.
api:
  - Ignis.Components.HeadlessUI.Listbox`1, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.ListboxButton, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.ListboxLabel, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.ListboxOptions, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.ListboxOption`1, Ignis.Components.HeadlessUI
---

## Basic usage

Listboxes are built using the `Listbox`, `ListboxButton`, `ListboxOptions`, `ListboxOption` and `ListboxLabel`
components.

The `ListboxButton` will automatically open/close the `ListboxOptions` when clicked, and when the menu is open, the list
of items receives focus and is automatically navigable via the keyboard.

## Binding objects as values

Unlike native HTML form controls which only allow you to provide strings as values, Headless UI supports binding complex
objects as well.

## Selecting multiple values

To allow selecting multiple values in your listbox, pass an array to the `Values` prop instead of using the `Value`
prop.

This will keep the listbox open when you are selecting options, and choosing an option will toggle it in place.

The `ValuesChanged` handler will be called with an array containing all selected options any time an option is added or
removed.

## Using a custom label

By default the `Listbox` will use the button contents as the label for screenreaders. If you'd like more control over
what is announced to assistive technologies, use the `ListboxLabel` component.

## Showing/hiding the listbox

By default, your `ListboxOptions` instance will be shown/hidden automatically based on the internal `IsOpen` state
tracked within the `Listbox` component itself.

## Transitions

To animate the opening/closing of the listbox panel, use the provided `Transition` component. All you need to do is wrap
the `ListboxOptions` in a `Transition`, and the transition will be applied automatically.

You can read more about transitions [here](/docs/components/headlessUI/transition).

## Rendering a different element for a component

By default, the `Listbox` and its subcomponents each render a default element that is sensible for that component.

For example, `ListboxLabel` renders a `label` by default, `ListboxButton` renders a `button`, `ListboxOptions` renders
a `ul`, and `ListboxOption` renders a `li`. By contrast, `Listbox` does not render an element, and instead renders its
children directly.

This is easy to change using the `AsElement` or `AsComponent` prop, which exists on every component.
You can read more about this [here](/docs/components/dynamic).

## Accessibility notes

### Focus management

When a Listbox is toggled open, the `ListboxOptions` receives focus. Focus is trapped within the list of items until
<kbd>Escape</kbd> is pressed or the user clicks outside the options. Closing the Listbox returns focus to
the `ListboxButton`.

### Mouse interaction

Clicking a `ListboxButton` toggles the options list open and closed. Clicking anywhere outside of the options list will
close the listbox.

### Keyboard interaction

| Command                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                | Description                                  |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | -------------------------------------------- |
| <kbd>Enter</kbd> <kbd>Space</kbd> <kbd><span class="sr-only">ArrowDown</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M4.095 3.578h2.808v5.28H8.38L5.5 12.422 2.62 8.858h1.476v-5.28z"></path></svg></kbd> <kbd><span class="sr-only">ArrowUp</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M6.903 12.422H4.095v-5.28H2.62L5.5 3.578l2.88 3.564H6.903v5.28z"></path></svg></kbd> when `ListboxButton` is focused. | Opens listbox and focuses the selected item. |
| <kbd>Escape</kbd> when listbox is open.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                | Closes listbox.                              |
| <kbd><span class="sr-only">ArrowDown</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M4.095 3.578h2.808v5.28H8.38L5.5 12.422 2.62 8.858h1.476v-5.28z"></path></svg></kbd> <kbd><span class="sr-only">ArrowUp</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M6.903 12.422H4.095v-5.28H2.62L5.5 3.578l2.88 3.564H6.903v5.28z"></path></svg></kbd> when listbox is open.                                              | Focuses previous/next item.                  |
| <kbd>Enter</kbd> <kbd>Space</kbd> when listbox is open.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                | Selects the current item.                    |

### Other

All relevant ARIA attributes are automatically managed.
