namespace Ignis.Components.Reactivity;

public sealed class ReactiveValue<T>
{
    private readonly IgnisComponentBase _owner;

    private T _value;

    public ReactiveValue(IgnisComponentBase owner, T value)
    {
        _value = value;
        _owner = owner ?? throw new ArgumentNullException(nameof(owner));
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

            _owner.Update();
        }
    }

    public void SetSilently(T value)
    {
        _value = value;
    }
}
