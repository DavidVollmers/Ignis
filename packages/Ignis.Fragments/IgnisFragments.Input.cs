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

        var context = new InputFragmentContext(value, async v => { await onInput(v == null ? default : (T)v); });

        var builder = TryGetFragmentBuilder<InputFragmentContext>(value) ?? new DefaultInputFragmentBuilder();

        return builder.BuildFragment(context);
    }

    public static RenderFragment? Input(object instance, PropertyInfo propertyInfo)
    {
        if (instance == null) throw new ArgumentNullException(nameof(instance));
        if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));

        var context = new InputFragmentContext(propertyInfo.CanRead ? propertyInfo.GetValue(instance) : null, v =>
        {
            if (!propertyInfo.CanWrite) return Task.CompletedTask;

            propertyInfo.SetValue(instance, v);

            return Task.CompletedTask;
        });

        var builder = TryGetFragmentBuilder<InputFragmentContext>(propertyInfo) ?? new DefaultInputFragmentBuilder();

        return builder.BuildFragment(context);
    }

    public static RenderFragment? Input<T>(Expression<Func<T>> expression)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));

        ParsePropertyExpression(expression, out var instance, out var propertyInfo);

        return Input(instance, propertyInfo);
    }
}