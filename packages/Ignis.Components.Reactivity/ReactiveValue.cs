namespace Ignis.Components.Reactivity;

public sealed class ReactiveValue<T> : ReactiveExpression where T : struct
{
    private T _value;

    //TODO introduce extension method for this
    public ReactiveValue(IgnisComponentBase owner, T value) : base(owner)
    {
        _value = value;
    }

    public T Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;

            Update();
        }
    }

    public void SetSilently(T value)
    {
        _value = value;
    }
}
