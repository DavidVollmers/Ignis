using Ignis.Fragments.Abstractions.Builder;
using Ignis.Fragments.Extensions;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Builder;

internal class DefaultFormFragmentBuilder : IFragmentBuilder<FormFragmentContext>
{
    public RenderFragment? BuildFragment(FormFragmentContext context)
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
            builder.AddAttribute(1, "onsubmit", OnSubmit);

            builder.AddContent(2, context.BuildFormProperties(property =>
            {
                // ReSharper disable once VariableHidesOuterVariable
                return builder => { builder.AddContent(3, IgnisFragments.Label(property)); };
            }));
            
            builder.OpenElement(4, "br");
            builder.CloseElement();
            
            builder.OpenElement(5, "input");
            builder.AddAttribute(6, "type", "submit");
            builder.AddAttribute(7, "value", "Submit");

            builder.CloseElement();

            builder.CloseElement();
        };
    }
}