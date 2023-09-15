---
order: 209
title: Tabs
category: HeadlessUI
permalink: /components/headlessUI/tabs
inject:
  type: Ignis.Website.Examples.HeadlessUI.TabsExample
  description: Easily create accessible, fully customizable tab interfaces, with robust focus management and keyboard navigation support.
api:
  - Ignis.Components.HeadlessUI.TabGroup, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.TabList, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.Tab, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.TabPanels, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.TabPanel, Ignis.Components.HeadlessUI
---

## Basic usage

Tabs are built using the `TabGroup`, `TabList`, `Tab`, `TabPanels`, and `TabPanel` components. By default the first tab
is selected, and clicking on any tab or selecting it with the keyboard will activate the corresponding panel.

## Specifying the default tab

To change which tab is selected by default, use the `DefaultIndex` prop on the `TabGroup` component.

## Listening for changes

To run a function whenever the selected tab changes, use the `SelectedIndexChanged` prop on the `TabGroup` component.

## Controlling the active tab

The tabs component can also be used as a controlled component. To do this, provide the `SelectedIndex` and manage the
state yourself.

## Rendering a different element for a component

By default, the `TabGroup` and its subcomponents each render a default element that is sensible for that component.

For example, `TabList` renders a `div` by default, `Tab` renders a `button`, `TabPanels` and `TabPanel` renders a `div`.
By contrast, `TabGroup` does not render an element, and instead renders its children directly.

This is easy to change using the `AsElement` or `AsComponent` prop, which exists on every component.
You can read more about this [here](/docs/components/dynamic).

## Accessibility notes

### Mouse interaction

Clicking a `Tab` will select that tab and display the corresponding `TabPanel`.

### Keyboard interaction

All interactions apply when a `Tab` component is focused.

| Command                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  | Description                    |
| ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------ |
| <kbd><span class="sr-only">ArrowLeft</span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M9.922 6.596v2.808h-5.28v1.476L1.078 8l3.564-2.88v1.476h5.28z"></path></svg></kbd> <kbd><span class="sr-only"></span><svg viewBox="0 0 11 16" fill="currentColor" xmlns="http://www.w3.org/2000/svg" aria-hidden="true" class="h-4 text-white"><path d="M1.078 9.404V6.596h5.28V5.12L9.922 8l-3.564 2.88V9.404h-5.28z"></path></svg></kbd> | Selects the previous/next tab. |

### Other

All relevant ARIA attributes are automatically managed.
