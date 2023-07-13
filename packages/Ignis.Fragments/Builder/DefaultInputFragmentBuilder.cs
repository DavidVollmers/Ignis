using Ignis.Fragments.Abstractions.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Builder;

internal class DefaultInputFragmentBuilder<T> : IFragmentBuilder<InputFragmentContext<T>>
{
    public RenderFragment? BuildFragment(InputFragmentContext<T> context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return builder =>
        {
            builder.OpenElement(0, "input");
            builder.AddAttribute(1, "type", "text");

            if (context.PropertyInfo != null)
            {
                builder.AddAttribute(2, "id", context.PropertyInfo.Name);
                builder.AddAttribute(3, "name", context.PropertyInfo.Name);
            }

            if (context.PropertyInfo?.CanWrite != true)
            {
                builder.AddAttribute(4, "readonly");
            }

            builder.AddAttribute(5, "value", BindConverter.FormatValue(context.Value));
            builder.AddAttribute(6, "onchange",
                EventCallback.Factory.CreateBinder<T>(this, v => context.OnInputAsync(v), context.Value!));

            builder.CloseElement();
        };
    }
}