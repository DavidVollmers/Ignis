---
order: 206
title: Dialog (Modal)
category: HeadlessUI
permalink: /components/headlessUI/dialog
inject:
  type: Ignis.Website.Examples.HeadlessUI.DialogExample
  description: A fully-managed, renderless dialog component jam-packed with accessibility and keyboard features, perfect for building completely custom modal and dialog windows for your next application.
api:
  - Ignis.Components.HeadlessUI.Dialog, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.DialogPanel, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.DialogTitle, Ignis.Components.HeadlessUI
  - Ignis.Components.HeadlessUI.DialogDescription, Ignis.Components.HeadlessUI
---

## Styling the dialog

Style the `Dialog` and `DialogPanel` components using the `class` or `style` props like you would with any other
element. You can also introduce additional elements if needed to achieve a particular design.

Clicking outside the `DialogPanel` component will close the dialog, so keep that in mind when deciding which styles to
apply to which elements.

## Adding a backdrop

If you'd like to add an overlay or backdrop behind your `DialogPanel` to bring attention to the panel itself, we
recommend using a dedicated element just for the backdrop and making it a sibling to your panel container:

## Transitions

To animate the opening/closing of the dialog, use the `Transition` component. All you need to do is wrap the `Dialog` in
a `Transition`, and dialog will transition automatically based on the state of the `Show` prop on the `Transition`.

When using `Transition` with your dialogs, you can remove the `IsOpen` prop, as the dialog will read the `Show` state
from the `Transition` automatically.

## Accessibility notes

### Mouse interaction

When a `Dialog` is rendered, clicking outside of the `DialogPanel` will close the `Dialog`.

No mouse interaction to open the `Dialog` is included out-of-the-box, though typically you will wire a `<button />`
element up with an `onclick` handler that toggles the Dialog's `IsOpen` prop to `true`.

### Other

All relevant ARIA attributes are automatically managed.
