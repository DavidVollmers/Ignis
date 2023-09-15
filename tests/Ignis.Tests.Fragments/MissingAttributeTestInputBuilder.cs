using Ignis.Fragments.Abstractions.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Tests.Fragments;

public class MissingAttributeTestInputBuilder : IFragmentBuilder<InputFragmentContext<string>>
{
    public RenderFragment? BuildFragment(InputFragmentContext<string> context)
    {
        return builder =>
        {
            builder.OpenElement(0, "input");
            builder.AddAttribute(1, "type", context.Attributes["type"] ?? "text");
            builder.CloseElement();
        };
    }
}
