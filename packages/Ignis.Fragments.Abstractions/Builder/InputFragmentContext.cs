using System.Reflection;

namespace Ignis.Fragments.Abstractions.Builder;

public sealed class InputFragmentContext
{
    public PropertyInfo? PropertyInfo { get; internal init; }

    public object? Value { get; }

    public Func<object?, Task> OnInputAsync { get; }

    internal InputFragmentContext(object? value, Func<object?, Task> onInput)
    {
        Value = value;
        OnInputAsync = onInput;
    }
}