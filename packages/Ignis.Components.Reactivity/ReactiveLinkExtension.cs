using Ignis.Components.Extensions;

namespace Ignis.Components.Reactivity;

internal class ReactiveLinkExtension : IComponentExtension
{
    private readonly IDictionary<IgnisComponentBase, ReactiveLink> _links =
        new Dictionary<IgnisComponentBase, ReactiveLink>();

    public void OnUpdate(IgnisComponentBase component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        if (_links.ContainsKey(component)) return;

        var link = _links[component];

        if (!link.Sections.Any()) return;

        link.Update();
    }

    public void OnDispose(IgnisComponentBase component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        _links.Remove(component);
    }

    public void AddLink(ReactiveLink link)
    {
        if (link == null) throw new ArgumentNullException(nameof(link));

        if (_links.ContainsKey(link.Owner))
            throw new InvalidOperationException("A component can only have one ReactiveLink.");

        _links[link.Owner] = link;
    }
}
