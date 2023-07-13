using System.Linq.Expressions;
using System.Reflection;
using Ignis.Fragments.Abstractions;
using Ignis.Fragments.Abstractions.Builder;

namespace Ignis.Fragments;

public static partial class IgnisFragments
{
    private static IFragmentBuilder<T>? TryGetFragmentBuilder<T>(object target) where T : class
    {
        return TryGetFragmentBuilder<T>(target.GetType());
    }

    private static IFragmentBuilder<T>? TryGetFragmentBuilder<T>(MemberInfo target) where T : class
    {
        var fragmentAttribute = target.GetCustomAttribute<FragmentAttribute>();
        return fragmentAttribute?.Builder as IFragmentBuilder<T>;
    }

    private static void ParsePropertyExpression<T>(Expression<Func<T>> expression, out PropertyInfo propertyInfo)
    {
        var body = expression.Body;

        if (body is UnaryExpression { NodeType: ExpressionType.Convert } unaryExpression &&
            unaryExpression.Type == typeof(object))
        {
            body = unaryExpression.Operand;
        }

        if (body is not MemberExpression { Member: PropertyInfo pi })
        {
            throw new ArgumentException(
                $"The provided expression contains a {body.GetType().Name} which is not supported. Only simple property accessors of an object are supported.",
                nameof(expression));
        }

        propertyInfo = pi;
    }
}