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
            builder.AddMultipleAttributes(1, context.Attributes!);
            builder.AddAttribute(2, "for", context.PropertyInfo.Name);

            builder.AddContent(3, context.GetPropertyDisplayName());

            builder.CloseElement();
        };
    }
}
