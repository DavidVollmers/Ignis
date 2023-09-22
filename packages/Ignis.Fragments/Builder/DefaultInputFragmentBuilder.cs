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
            if (context.PropertyInfo != null) builder.AddAttribute(1, "name", context.PropertyInfo.Name);
            builder.AddAttribute(2, "type", "text");
            builder.AddMultipleAttributes(3, context.Attributes!);
            if (context.PropertyInfo != null) builder.AddAttribute(4, "id", context.PropertyInfo.Name);
            if (context.PropertyInfo?.CanWrite != true)
            {
                builder.AddAttribute(5, "readonly");
            }

            builder.AddAttribute(6, "value", BindConverter.FormatValue(context.Value));
            builder.AddAttribute(7, "onchange",
                EventCallback.Factory.CreateBinder<T>(this, v => _ = context.OnInputAsync(v), context.Value!));

            builder.CloseElement();
        };
    }
}
