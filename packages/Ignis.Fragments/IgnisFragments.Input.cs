using System.Linq.Expressions;
using System.Reflection;
using Ignis.Fragments.Abstractions.Builder;
using Ignis.Fragments.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments;

public static partial class IgnisFragments
{
    public delegate void OnInput();

    public delegate void OnInput<in T>(T? value);

    public delegate Task OnInputAsync();

    public delegate Task OnInputAsync<in T>(T? value);

    public static RenderFragment? Input(object? value, OnInput onInput)
    {
        if (onInput == null) throw new ArgumentNullException(nameof(onInput));

        return Input(value, () =>
        {
            onInput();
            return Task.CompletedTask;
        });
    }

    public static RenderFragment? Input<T>(T? value, OnInput<T> onInput)
    {
        if (onInput == null) throw new ArgumentNullException(nameof(onInput));

        return Input(value, v =>
        {
            onInput(v);
            return Task.CompletedTask;
        });
    }

    public static RenderFragment? Input(object? value, OnInputAsync onInput)
    {
        if (onInput == null) throw new ArgumentNullException(nameof(onInput));

        return Input(value, async _ => { await onInput(); });
    }

    public static RenderFragment? Input<T>(T? value, OnInputAsync<T> onInput)
    {
        if (onInput == null) throw new ArgumentNullException(nameof(onInput));

        var context =
            new InputFragmentContext<T>(() => value, async v => { await onInput(value = v == null ? default : (T)v); });

        var builder = TryGetFragmentBuilder<InputFragmentContext<T>>(value) ?? new DefaultInputFragmentBuilder<T>();

        return builder.BuildFragment(context);
    }

    public static RenderFragment? Input<T>(Expression<Func<T>> expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        ParsePropertyExpression(expression, out var instance, out var propertyInfo);

        return Input(instance, propertyInfo);
    }

    public static RenderFragment? Input(object instance, PropertyInfo propertyInfo)
    {
        if (instance == null) throw new ArgumentNullException(nameof(instance));
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        var method = typeof(IgnisFragments).GetMethod(nameof(InputCore), BindingFlags.NonPublic | BindingFlags.Static)!;

        var genericMethod = method.MakeGenericMethod(propertyInfo.PropertyType);

        return (RenderFragment?)genericMethod.Invoke(null, new[] { instance, propertyInfo });
    }

    private static RenderFragment? InputCore<T>(object instance, PropertyInfo propertyInfo)
    {
        var context = new InputFragmentContext<T>(
            () => propertyInfo.CanRead ? (T)propertyInfo.GetValue(instance)! : default,
            v =>
            {
                if (!propertyInfo.CanWrite) return Task.CompletedTask;

                propertyInfo.SetValue(instance, v);

                return Task.CompletedTask;
            }) { PropertyInfo = propertyInfo };

        var builder = TryGetFragmentBuilder<InputFragmentContext<T>>(propertyInfo) ??
                      new DefaultInputFragmentBuilder<T>();

        return builder.BuildFragment(context);
    }
}
