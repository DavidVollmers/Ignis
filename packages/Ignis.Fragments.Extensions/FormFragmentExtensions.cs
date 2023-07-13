using System.Reflection;
using Ignis.Fragments.Abstractions.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Extensions;

public static class FormFragmentExtensions
{
    public static RenderFragment BuildFormProperties(this IFragmentBuilder<FormFragmentContext> fragmentBuilder,
        int sequence, FormFragmentContext context, Func<PropertyInfo, RenderFragment?> propertyFragmentBuilder)
    {
        if (fragmentBuilder == null) throw new ArgumentNullException(nameof(fragmentBuilder));
        if (context == null) throw new ArgumentNullException(nameof(context));

        return builder =>
        {
            var properties = context.Model.GetType().GetProperties();

            foreach (var property in properties)
            {
                builder.AddContent(sequence, propertyFragmentBuilder(property));
            }
        };
    }
}