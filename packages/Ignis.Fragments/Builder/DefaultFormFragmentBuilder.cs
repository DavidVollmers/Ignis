using Ignis.Fragments.Abstractions.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Builder;

internal class DefaultFormFragmentBuilder : IFragmentBuilder<FormFragmentContext>
{
    public RenderFragment BuildFragment(FormFragmentContext context)
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

            builder.CloseElement();
        };
    }
}