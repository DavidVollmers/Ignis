using System.Reflection;
using Ignis.Fragments.Abstractions.Builder;

namespace Ignis.Fragments.Extensions;

public static class FormFragmentExtensions
{
    public static PropertyInfo[] GetProperties<T>(this FormFragmentContext<T> context) where T : class
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return context.Model.GetType().GetProperties();
    }
}
