namespace Ignis.Components.Reactivity;

public abstract class ReactiveExpression<T>
{
    private readonly IList<ReactiveSection<T>> _sections = new List<ReactiveSection<T>>();
    private readonly IgnisComponentBase _owner;

    protected ReactiveExpression(IgnisComponentBase owner)
    {
        _owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    protected void Update()
    {
        if (_sections.Any())
        {
            foreach (var section in _sections)
            {
                section.Update();
            }
        }
        else _owner.Update();
    }

    internal void Subscribe(ReactiveSection<T> section)
    {
        if (_sections.Contains(section)) return;

        _sections.Add(section);
    }

    internal void Unsubscribe(ReactiveSection<T> section)
    {
        _sections.Remove(section);
    }
}
