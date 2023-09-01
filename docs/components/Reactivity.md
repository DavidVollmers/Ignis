---
order: 3
title: Reactivity
category: Components
permalink: /components/reactivity
---

When you build your Ignis components you have full control over how they react to changes in their state. This is done
by manually calling the `Update` method on the component. But this also means you need to call the `Update` method on
every internal state changed which is not coming from a `Parameter` or `CascadingParameter`.

To make this easier, Ignis provides the `ReactiveValue<T>` class which will automatically call the `Update` method on
the component when the value is changed.

```cshtml
@using Ignis.Components
@using Ignis.Components.Reactivity

@inherits IgnisComponentBase

<p>@_counter.Value</p>

<button @onclick="@(() => ++_counter.Value)">Increment</button>

@code
{
    private readonly ReactiveValue<int> _counter;

    public ReactiveCounter()
    {
        _counter = new ReactiveValue<int>(this, 0);
    }
}
```

*This will render a paragraph with the value `0` and a button with the text `Increment`. When the button is clicked, the
paragraph will be updated with the new value.*

## Reactive sections

When building larger components, you might want to only update a specific section of your component. This can be done
by using the `ReactiveSection` component.

```cshtml
@using Ignis.Components
@using Ignis.Components.Reactivity

@inherits IgnisComponentBase

<ReactiveSection AsElement="p" Value="_counter1" id="counter-1">
    @_counter1.Value
</ReactiveSection>
<ReactiveSection AsElement="p" Value="_counter2" id="counter-2">
    @_counter2.Value
</ReactiveSection>

<button @onclick="@AlternateIncrement">Increment</button>

@code
{
    private readonly ReactiveValue<int> _counter1;
    private readonly ReactiveValue<int> _counter2;

    public AlternatingCounter()
    {
        _counter1 = new ReactiveValue<int>(this, 0);
        _counter2 = new ReactiveValue<int>(this, 0);
    }

    private void AlternateIncrement()
    {
        if (_counter1.Value > _counter2.Value)
            _counter2.Value++;
        else
            _counter1.Value++;
    }
}
```

*This will render two paragraphs with the value `0` and a button with the text `Increment`. When the button is clicked,
the paragraph with the lowest value will be updated with the new value.*