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

## Accessibility notes

### Mouse interaction

Clicking a `DisclosureButton` toggles the Disclosure's panel open and closed.

### Keyboard interaction

| Command                                                                 | Description    |
|-------------------------------------------------------------------------|----------------|
| <kbd>Enter</kbd> <kbd>Space</kbd> when a `DisclosureButton` is focused. | Toggles panel. |

### Other

All relevant ARIA attributes are automatically managed.
