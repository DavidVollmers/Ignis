namespace Ignis.Components.Reactivity;

public abstract class ReactiveExpression
{
    private readonly IList<ReactiveSection> _sections = new List<ReactiveSection>();
    private readonly IgnisComponentBase _owner;

    protected ReactiveExpression(IgnisComponentBase owner)
    {
        _owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    protected internal void Update()
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

    internal void Subscribe(ReactiveSection section)
    {
        if (_sections.Contains(section)) return;

        _sections.Add(section);
    }

    internal void Unsubscribe(ReactiveSection section)
    {
        _sections.Remove(section);
    }
}
