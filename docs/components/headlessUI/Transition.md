---
order: 210
title: Transition
category: HeadlessUI
permalink: /components/headlessUI/transition
inject:
  type: Ignis.Website.Examples.HeadlessUI.TransitionExample
  description: The Transition component lets you add enter/leave transitions to conditionally rendered elements, using CSS classes to control the actual transition styles in the different stages of the transition.
api:
  - Ignis.Components.HeadlessUI.Transition, Ignis.Components.HeadlessUI
---

## Showing and hiding content

Wrap the content that should be conditionally rendered in a `Transition` component, and use the `Show` prop to control
whether the content should be visible or hidden.

## Animating transitions

By default, a `Transition` will enter and leave instantly, which is probably not what you're looking for if you're using
this component.

To animate your enter/leave transitions, add classes that provide the styling for each phase of the transitions using
these props:

- **Enter**: Applied the entire time an element is entering. Usually you define your duration and what properties you
  want to transition here, for example `transition-opacity duration-75`.
- **EnterFrom**: The starting point to enter from, for example `opacity-0` if something should fade in.
- **EnterTo**: The ending point to enter to, for example `opacity-100` after fading in.
- **Leave**: Applied the entire time an element is leaving. Usually you define your duration and what properties you
  want to transition here, for example `transition-opacity duration-75`.
- **LeaveFrom**: The starting point to leave from, for example `opacity-100` if something should fade out.
- **LeaveTo**: The ending point to leave to, for example `opacity-0` after fading out.

## Transitioning on initial render

If you want an element to transition the very first time it's rendered, set the `Appear` prop to `true`.

This is useful if you want something to transition in on initial page load, or when its parent is conditionally
rendered.

## Rendering a different element for a component

By default, the `Transition` component does not render an element, and instead renders its children directly.

This is easy to change using the `AsElement` or `AsComponent` prop, which exists on every component.
You can read more about this [here](/components/dynamic).
