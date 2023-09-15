using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Ignis.Fragments.Abstractions.Builder;

namespace Ignis.Fragments.Extensions;

public static class LabelFragmentExtensions
{
    public static string GetPropertyDisplayName(this LabelFragmentContext context, string? defaultValue = null)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.PropertyInfo.GetCustomAttribute<DisplayAttribute>()?.GetName() ??
               context.PropertyInfo.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ??
               defaultValue ??
               context.PropertyInfo.Name;
    }
}
