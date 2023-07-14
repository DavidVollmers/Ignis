using Ignis.Fragments.Abstractions.Builder;
using Ignis.Fragments.Extensions;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Builder;

internal class DefaultFormFragmentBuilder<T> : IFragmentBuilder<FormFragmentContext<T>> where T : class
{
    public RenderFragment? BuildFragment(FormFragmentContext<T> context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        async Task OnSubmit()
        {
            //TODO validation

            await context.OnSubmitAsync(context.Model);
        }

        return builder =>
        {
            builder.OpenElement(0, "form");
            builder.AddMultipleAttributes(1, context.Attributes!);
            builder.AddAttribute(2, "onsubmit", OnSubmit);

            // ReSharper disable once VariableHidesOuterVariable
            builder.AddContent(3, builder =>
            {
                foreach (var property in context.GetProperties())
                {
                    builder.AddContent(4, IgnisFragments.Label(property));

                    builder.OpenElement(5, "br");
                    builder.CloseElement();

                    builder.AddContent(6, IgnisFragments.Input(context.Model, property));

                    builder.OpenElement(7, "br");
                    builder.CloseElement();
                }
            });

            builder.OpenElement(8, "br");
            builder.CloseElement();

            builder.OpenElement(9, "input");
            builder.AddAttribute(10, "type", "submit");
            builder.AddAttribute(11, "value", "Submit");

            builder.CloseElement();

            builder.CloseElement();
        };
    }
}
