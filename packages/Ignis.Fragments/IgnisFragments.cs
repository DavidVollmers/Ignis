using System.Linq.Expressions;
using System.Reflection;
using Ignis.Fragments.Abstractions;
using Ignis.Fragments.Abstractions.Builder;

namespace Ignis.Fragments;

public static partial class IgnisFragments
{
    private static AttributeCollection GetAttributes(object? target)
    {
        return target == null ? new AttributeCollection() : GetAttributes(target.GetType());
    }

    private static AttributeCollection GetAttributes(MemberInfo target)
    {
        var attributeAttributes = target.GetCustomAttributes<AttributeAttribute>();
        var dictionary = attributeAttributes.ToDictionary(a => a.Name, a => a.Value, StringComparer.OrdinalIgnoreCase);
        return new AttributeCollection(dictionary);
    }

    private static IFragmentBuilder? TryGetFragmentBuilder<T>(object? target) where T : class
    {
        return target == null ? null : TryGetFragmentBuilder<T>(target.GetType());
    }

    private static IFragmentBuilder? TryGetFragmentBuilder<T>(MemberInfo target) where T : class
    {
        var fragmentAttributes = target.GetCustomAttributes<RenderAsAttribute>();
        foreach (var fragmentAttribute in fragmentAttributes)
        {
            if (fragmentAttribute.GetBuilder() is IFragmentBuilder<T> builder)
            {
                return builder;
            }
        }

        return null;
    }

    private static void ParsePropertyExpression<T>(Expression<Func<T>> expression, out object instance,
        out PropertyInfo propertyInfo)
    {
        var body = expression.Body;

        if (body is UnaryExpression { NodeType: ExpressionType.Convert } unaryExpression &&
            unaryExpression.Type == typeof(object))
        {
            body = unaryExpression.Operand;
        }

        if (body is not MemberExpression { Member: PropertyInfo pi } memberExpression)
        {
            throw new ArgumentException(
                $"The provided expression contains a {body.GetType().Name} which is not supported. Only simple property accessors of an object are supported.",
                nameof(expression));
        }

        propertyInfo = pi;

        if (memberExpression.Expression is ConstantExpression constantExpression)
        {
            instance = constantExpression.Value ??
                       throw new ArgumentException("The provided expression must evaluate to a non-null value.",
                           nameof(expression));
        }
        else if (memberExpression.Expression != null)
        {
            var instanceLambda = Expression.Lambda(memberExpression.Expression);
            var instanceLambdaCompiled = (Func<object?>)instanceLambda.Compile();
            var result = instanceLambdaCompiled();
            instance = result ??
                       throw new ArgumentException("The provided expression must evaluate to a non-null value.",
                           nameof(expression));
        }
        else
        {
            throw new ArgumentException(
                $"The provided expression contains a {body.GetType().Name} which is not supported. Only simple property accessors of an object are supported.",
                nameof(expression));
        }
    }
}
