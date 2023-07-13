using Ignis.Fragments.Abstractions.Builder;
using Ignis.Fragments.Extensions;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Builder;

internal class DefaultLabelFragmentBuilder : IFragmentBuilder<LabelFragmentContext>
{
    public RenderFragment? BuildFragment(LabelFragmentContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return builder =>
        {
            builder.OpenElement(0, "label");
            builder.AddAttribute(1, "for", context.PropertyInfo.Name);

            builder.AddContent(2, context.GetPropertyDisplayName());

            builder.CloseElement();
        };
    }
}