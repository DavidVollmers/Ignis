using System.Reflection;
using Ignis.Fragments.Abstractions.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Extensions;

public static class FormFragmentExtensions
{
    public static RenderFragment BuildFormProperties(this FormFragmentContext context,
        Func<PropertyInfo, RenderFragment?> propertyFragmentBuilder)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return builder =>
        {
            var properties = context.Model.GetType().GetProperties();

            foreach (var property in properties)
            {
                propertyFragmentBuilder(property)?.Invoke(builder);
            }
        };
    }
}