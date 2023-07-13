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
                return builder =>
                {
                    builder.AddContent(3, IgnisFragments.Label(property));
                    
                    builder.OpenElement(4, "br");
                    builder.CloseElement();
                    
                    builder.AddContent(5, IgnisFragments.Input(context.Model, property));
                    
                    builder.OpenElement(6, "br");
                    builder.CloseElement();
                };
            }));
            
            builder.OpenElement(7, "br");
            builder.CloseElement();
            
            builder.OpenElement(8, "input");
            builder.AddAttribute(9, "type", "submit");
            builder.AddAttribute(10, "value", "Submit");

            builder.CloseElement();

            builder.CloseElement();
        };
    }
}