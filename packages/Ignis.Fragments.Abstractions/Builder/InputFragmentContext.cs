using System.Reflection;

namespace Ignis.Fragments.Abstractions.Builder;

public sealed class InputFragmentContext<T>
{
    public PropertyInfo? PropertyInfo { get; internal init; }

    public T? Value { get; }

    public Func<T?, Task> OnInputAsync { get; }

    internal InputFragmentContext(T? value, Func<T?, Task> onInput)
    {
        Value = value;
        OnInputAsync = onInput;
    }
}