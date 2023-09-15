using System.Linq.Expressions;
using System.Reflection;
using Ignis.Fragments.Abstractions.Builder;
using Ignis.Fragments.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments;

public static partial class IgnisFragments
{
    public delegate void OnInput<in T>(T? value);

    public delegate Task OnInputAsync<in T>(T? value);

    public static RenderFragment? Input<T>(T? value, OnInput<T> onInput,
        IFragmentBuilder<InputFragmentContext<T>>? defaultBuilder = null)
    {
        if (onInput == null) throw new ArgumentNullException(nameof(onInput));

        return Input(value, v =>
        {
            onInput(v);
            return Task.CompletedTask;
        }, defaultBuilder);
    }

    public static RenderFragment? Input<T>(T? value, OnInputAsync<T> onInput,
        IFragmentBuilder<InputFragmentContext<T>>? defaultBuilder = null)
    {
        if (onInput == null) throw new ArgumentNullException(nameof(onInput));

        var context =
            new InputFragmentContext<T>(() => value, async v => { await onInput(value = v == null ? default : (T)v); })
            {
                Attributes = GetAttributes(value)
            };

        var builder = TryGetFragmentBuilder<InputFragmentContext<T>>(value) ??
                      defaultBuilder ?? new DefaultInputFragmentBuilder<T>();

        return builder.BuildFragment(context);
    }

    public static RenderFragment? Input<T>(Expression<Func<T>> expression,
        IFragmentBuilder<InputFragmentContext<T>>? defaultBuilder = null)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        ParsePropertyExpression(expression, out var instance, out var propertyInfo);

        return Input(instance, propertyInfo, defaultBuilder);
    }

    public static RenderFragment? Input(object instance, PropertyInfo propertyInfo,
        IFragmentBuilder? defaultBuilder = null)
    {
        if (instance == null) throw new ArgumentNullException(nameof(instance));
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        if (defaultBuilder != null)
        {
            var genericBuilderInterface = defaultBuilder.GetType().GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IFragmentBuilder<>));
            if (genericBuilderInterface == null ||
                genericBuilderInterface.GenericTypeArguments[0]
                    .GetGenericTypeDefinition() != typeof(InputFragmentContext<>) ||
                genericBuilderInterface.GenericTypeArguments[0].GenericTypeArguments[0] != propertyInfo.PropertyType)
            {
                throw new ArgumentException(
                    $"The default builder must implement {typeof(IFragmentBuilder<>).Name}<{typeof(InputFragmentContext<>)}<{propertyInfo.PropertyType.Name}>>.",
                    nameof(defaultBuilder));
            }
        }

        var method = typeof(IgnisFragments).GetMethod(nameof(InputCore), BindingFlags.NonPublic | BindingFlags.Static)!;

        var genericMethod = method.MakeGenericMethod(propertyInfo.PropertyType);

        return (RenderFragment?)genericMethod.Invoke(null, new[] { instance, propertyInfo, defaultBuilder });
    }

    private static RenderFragment? InputCore<T>(object instance, PropertyInfo propertyInfo,
        // ReSharper disable once SuggestBaseTypeForParameter
        IFragmentBuilder<InputFragmentContext<T>>? defaultBuilder = null)
    {
        var context = new InputFragmentContext<T>(
            () => propertyInfo.CanRead ? (T)propertyInfo.GetValue(instance)! : default,
            v =>
            {
                if (!propertyInfo.CanWrite) return Task.CompletedTask;

                propertyInfo.SetValue(instance, v);

                return Task.CompletedTask;
            })
        { Attributes = GetAttributes(propertyInfo), PropertyInfo = propertyInfo };

        var builder = TryGetFragmentBuilder<InputFragmentContext<T>>(propertyInfo) ??
                      defaultBuilder ?? new DefaultInputFragmentBuilder<T>();

        return builder.BuildFragment(context);
    }
}
