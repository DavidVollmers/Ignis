namespace Ignis.Components.Reactivity;

public sealed class ReactiveReference<T> : ReactiveExpression where T : class?
{
    private readonly Action<T> _setter;
    private readonly Func<T> _getter;

    public ReactiveReference(IgnisComponentBase owner, Func<T> getter, Action<T> setter) : base(owner)
    {
        _getter = getter ?? throw new ArgumentNullException(nameof(getter));
        _setter = setter ?? throw new ArgumentNullException(nameof(setter));
    }

    public T Value
    {
        get
        {
            return _getter();
        }
        set
        {
            _setter(value);

            Update();
        }
    }

    public void SetSilently(T value)
    {
        _setter(value);
    }
}
