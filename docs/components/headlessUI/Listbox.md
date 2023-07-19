---
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

## Accessibility notes

### Focus management

When a Listbox is toggled open, the `ListboxOptions` receives focus. Focus is trapped within the list of items until
<kbd>Escape</kbd> is pressed or the user clicks outside the options. Closing the Listbox returns focus to
the `ListboxButton`.

### Mouse interaction

Clicking a `ListboxButton` toggles the options list open and closed. Clicking anywhere outside of the options list will
close the listbox.

### Keyboard interaction

| Command                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               | Description                                 |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------|
| <kbd>Enter</kbd> <kbd>Space</kbd> <kbd><span class="sr-only">ArrowDown</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M4.095 3.578h2.808v5.28H8.38L5.5 12.422 2.62 8.858h1.476v-5.28z"></path></svg></kbd> <kbd><span class="sr-only">ArrowUp</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M6.903 12.422H4.095v-5.28H2.62L5.5 3.578l2.88 3.564H6.903v5.28z"></path></svg></kbd> when `ListboxButton` is focused | Opens listbox and focuses the selected item |
| <kbd>Escape</kbd> when listbox is open                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                | Closes listbox                              |
| <kbd><span class="sr-only">ArrowDown</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M4.095 3.578h2.808v5.28H8.38L5.5 12.422 2.62 8.858h1.476v-5.28z"></path></svg></kbd> <kbd><span class="sr-only">ArrowUp</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M6.903 12.422H4.095v-5.28H2.62L5.5 3.578l2.88 3.564H6.903v5.28z"></path></svg></kbd> when listbox is open                                              | Focuses previous/next item                  |
| <kbd>Enter</kbd> <kbd>Space</kbd> when listbox is open                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                | Selects the current item                    |

### Other

All relevant ARIA attributes are automatically managed.
