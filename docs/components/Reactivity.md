---
order: 3
title: Reactivity
category: Components
permalink: /components/reactivity
---

When you build your Ignis components you have full control over how they react to changes in their state. This is done
by manually calling the `Update` method on the component. But this also means you need to call the `Update` method on
every internal state change which is not coming from a `Parameter` or `CascadingParameter`.

To make this easier, Ignis provides the `ReactiveValue<T>` and `ReactiveReference<T>` classes which will automatically
call the `Update` method on the component when the value or reference is changed.

```cshtml
@using Ignis.Components
@using Ignis.Components.Reactivity

@inherits IgnisComponentBase

<p>@_counter.Value</p>

<button @onclick="@(() => ++_counter.Value)">Increment</button>

@code
{
    private readonly ReactiveValue<int> _counter;

    // We need to create the ReactiveValue in the constructor of the component.
    public ReactiveCounter()
    {
        _counter = new ReactiveValue<int>(this, 0);
    }
}
```

_This will render a paragraph with the value `0` and a button with the text `Increment`. When the button is clicked, the
paragraph will be updated with the new value._

## Reactive sections

When building larger components, you might want to only update a specific section of your component. This can be done
by using the `ReactiveSection` component.

```cshtml
@using Ignis.Components
@using Ignis.Components.Reactivity

@inherits IgnisComponentBase

<ReactiveSection AsElement="p" For="_counter1">
    Counter 1: @_counter1.Value
</ReactiveSection>
<ReactiveSection AsElement="p" For="_counter2">
    Counter 2: @_counter2.Value
</ReactiveSection>

<button @onclick="@(() => ++_counter1.Value)">Increment Counter 1</button>
<button @onclick="@(() => ++_counter2.Value)">Increment Counter 2</button>

@code
{
    private readonly ReactiveValue<int> _counter1;
    private readonly ReactiveValue<int> _counter2;

    // We need to create the ReactiveValue in the constructor of the component.
    public ReactiveSectionCounter()
    {
        _counter1 = new ReactiveValue<int>(this, 0);
        _counter2 = new ReactiveValue<int>(this, 0);
    }
}
```

_This will render two paragraphs with the values `0` and two buttons with the text `Increment Counter 1` and
`Increment Counter 2`. When the buttons are clicked, only the paragraph with the corresponding value will be updated
**and not the whole component**._
