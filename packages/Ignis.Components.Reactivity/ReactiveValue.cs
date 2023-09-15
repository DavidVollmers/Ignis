namespace Ignis.Components.Reactivity;

public sealed class ReactiveValue<T>
{
    private readonly IList<ReactiveSection<T>> _sections = new List<ReactiveSection<T>>();
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

            if (_sections.Any())
            {
                foreach (var section in _sections)
                {
                    section.Update();
                }
            }
            else _owner.Update();
        }
    }

    public void SetSilently(T value)
    {
        _value = value;
    }

    internal void Adopt(ReactiveSection<T> section)
    {
        if (_sections.Contains(section)) return;

        _sections.Add(section);
    }

    internal void SetFree(ReactiveSection<T> section)
    {
        _sections.Remove(section);
    }
}
