using System.Linq.Expressions;
using System.Reflection;
using Ignis.Fragments.Abstractions.Builder;
using Ignis.Fragments.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments;

public static partial class IgnisFragments
{
    public static RenderFragment? Label(PropertyInfo propertyInfo,
        IFragmentBuilder<LabelFragmentContext>? defaultBuilder = null)
    {
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        var context = new LabelFragmentContext(propertyInfo) { Attributes = GetAttributes(propertyInfo) };

        var builder = TryGetFragmentBuilder<LabelFragmentContext>(propertyInfo) ??
                      defaultBuilder ?? new DefaultLabelFragmentBuilder();

        return builder.BuildFragment(context);
    }

    public static RenderFragment? Label<T>(Expression<Func<T>> expression,
        IFragmentBuilder<LabelFragmentContext>? defaultBuilder = null)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        ParsePropertyExpression(expression, out _, out var propertyInfo);

        return Label(propertyInfo, defaultBuilder);
    }
}
