namespace Ignis.Components.Reactivity;

public abstract class ReactiveExpression
{
    private readonly IList<ReactiveSection> _sections = new List<ReactiveSection>();

    internal IEnumerable<ReactiveSection> Sections => _sections;

    internal IgnisComponentBase Owner { get; }

    protected ReactiveExpression(IgnisComponentBase owner)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
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
        else Owner.Update();
    }

    protected internal virtual void Initialize()
    {
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
