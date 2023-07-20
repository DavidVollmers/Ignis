---
order: 1
title: Dynamic Components
category: Components
permalink: /components/dynamic
---

Dynamic components can be rendered dynamically as either a component or an element. This is useful when you need to
render a component based on a variable value or want more flexibility on how your component is used by others.

You can control how to render a dynamic component by setting either the `AsElement` string property with the name of the
HTML tag to render or the `AsComponent` property with the type of the component to render.

Out of the box, the `Ignis.Components` package provides the `Dynamic` component which can be used to render any element
or component:

```cshtml
@using Ignis.Components

<Dynamic AsElement="div" class="container">
    <Dynamic AsElement="h1">Hello World!</Dynamic>
</Dynamic>
```

## Dynamic components with child content



## Designing a dynamic component

Each dynamic component **must** implement the `IDynamicComponent` interface. If the component can contain child content
it **should** also implement the `IDynamicParentComponent` interface.


