namespace Ignis.Components;

internal class OutletRegistry : IOutletRegistry
{
    private readonly IList<IOutletComponent> _components = new List<IOutletComponent>();

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

    public IEnumerable<IOutletComponent> GetFreeComponents()
    {
        return _components.Where(component => !component.IsAdopted);
    }
}
