namespace Ignis.Components;

internal class OutletRegistry : IOutletRegistry
{
    private readonly IList<IOutlet> _outlets = new List<IOutlet>();
    private readonly IList<IOutletComponent> _components = new List<IOutletComponent>();

    public void AddOutlet(IOutlet outlet)
    {
        if (outlet == null) throw new ArgumentNullException(nameof(outlet));

        if (!_outlets.Contains(outlet)) _outlets.Add(outlet);
    }

    public void RemoveOutlet(IOutlet outlet)
    {
        if (outlet == null) throw new ArgumentNullException(nameof(outlet));

        _outlets.Remove(outlet);
    }

    public void RegisterComponent(IOutletComponent component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        if (!_components.Contains(component)) _components.Add(component);
    }

    public void UnregisterComponent(IOutletComponent component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        _components.Remove(component);
    }

    public bool HasOutletFor(IOutletComponent component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        return _components.Contains(component) && _outlets.Any(outlet => outlet.CanOutlet(component));
    }
}
