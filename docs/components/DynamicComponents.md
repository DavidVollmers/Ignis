---
order: 2
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
<Dynamic AsElement="div" class="container">
    <Dynamic AsElement="h1">Hello World!</Dynamic>
</Dynamic>
```

_This will render a `div` element with a `container` class and a `h1` element with the text `Hello World!`._

## Dynamic components rendered as fragments

When rendering a dynamic component as a `Fragment`, the component will not be rendered and instead only its child
content will be rendered.

```cshtml
<Dynamic AsComponent="typeof(Fragment)" class="container">
    <Dynamic AsElement="h1">Hello World!</Dynamic>
</Dynamic>
```

_This will **only** render a `h1` element with the text `Hello World!`._

If a dynamic component is not meant to be rendered as a fragment per default, doing so might break its functionality or
appearance. To still support rendering as a fragment, the component **must** implement the `IDynamicParentComponent`
interface or the `IDynamicParentComponent<T>` interface. This will allow users to render child content via the `_`
render fragment and provide required attributes and reference captures as they see fit.

```cshtml
<Dynamic AsComponent="typeof(Fragment)" class="container">
    <_ Context="container">
        <Dynamic AsElement="h1" @attributes="container.Attributes">Hello World!</Dynamic>
    </_>
</Dynamic>
```

_This will render a `h1` element with the text `Hello World!` **and** the `container` class._

## Designing a dynamic component

Each dynamic component **must** implement the `IDynamicComponent` interface. If the component can contain child content
it **should** also implement the `IDynamicParentComponent` interface or the `IDynamicParentComponent<T>` interface when
providing context to the child content.

You **should** set a default for either the `AsElement` or `AsComponent` property. This will allow users to render your
dynamic component without caring about the dynamic part.

To build the render tree of a dynamic component, you **can** use
the `OpenAs`, `CloseAs`, `AddContentFor`, `AddChildContentFor<TContext, TDynamic>` methods.

```csharp
// Example implmentation of the <Dynamic> component. (This is deviating from the actual implementation)
public sealed class Dynamic : IgnisComponentBase, IDynamicParentComponent<Dynamic>
{
    private Type? _asComponent;
    private string? _asElement;

    // Make sure to only allow setting either the AsElement or AsComponent property.
    // Otherwise an exception will be thrown when rendering the component.
    [Parameter]
    public string? AsElement
    {
        get => _asElement;
        set
        {
            _asElement = value;
            _asComponent = null;
        }
    }

    [Parameter]
    public Type? AsComponent
    {
        get => _asComponent;
        set
        {
            _asComponent = value;
            _asElement = null;
        }
    }

    // The _ render fragment which can be used to provide attributes and reference capture of this component to a child.
    [Parameter] public RenderFragment<IDynamicComponent>? _ { get; set; }

    // The default render fragment for child content.
    [Parameter] public RenderFragment? ChildContent { get; set; }

    // Capture all unmatched parameters and attributes to pass them to the rendered child content.
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    // Optional property to allow capturing element references.
    public ElementReference? Element { get; set; }

    // Optional property to allow capturing component references.
    public object? Component { get; set; }

    // This must return all necessary attributes and uncaptured values of your component so they can be provided to a child.
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    // Make sure to provide a default for either the AsElement or AsComponent property.
    public Dynamic()
    {
        AsComponent = typeof(Fragment);
    }

    // Renders the dynamic component using IgnisComponentExtensions.
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<IDynamicComponent, Dynamic>(2, this, ChildContent);

        builder.CloseAs(this);
    }
}
```

You can also use the `DynamicComponentBase` class to simplify the implementation of a dynamic component.

```csharp
public sealed class Dynamic : DynamicComponentBase<Dynamic>
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    // Make sure to provide a default for either the AsElement or AsComponent property via the base constructor.
    public Dynamic() : base(typeof(Fragment))
    {
        // Use the SetAttributes method to set the attributes of your component. (Even if there are none)
        SetAttributes(ArraySegment<Func<KeyValuePair<string, object?>>>.Empty);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor(2, this, ChildContent);

        builder.CloseAs(this);
    }
}
```
