using Ignis.Fragments.Abstractions.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Tests.Fragments;

public class TestInputBuilder<T> : IFragmentBuilder<InputFragmentContext<T>>
{
    private readonly string _value;

    public TestInputBuilder(string value)
    {
        _value = value;
    }

    public RenderFragment? BuildFragment(InputFragmentContext<T> context)
    {
        return builder =>
        {
            builder.OpenElement(0, "input");
            builder.AddAttribute(1, "value", _value);
            builder.CloseElement();
        };
    }
}
