using System.Reflection;

namespace Ignis.Fragments.Abstractions.Builder;

public class InputFragmentContext<T> : FragmentContext
{
    private readonly Func<T?> _valueFunc;

    public PropertyInfo? PropertyInfo { get; internal init; }

    public T? Value => _valueFunc();

    public Func<T?, Task> OnInputAsync { get; }

    internal InputFragmentContext(Func<T?> valueFunc, Func<T?, Task> onInput)
    {
        _valueFunc = valueFunc;
        OnInputAsync = onInput;
    }
}
