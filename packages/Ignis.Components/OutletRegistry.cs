namespace Ignis.Components;

internal class OutletRegistry : IOutletRegistry
{
    private readonly IList<IOutletComponent> _components = new List<IOutletComponent>();
    private readonly IList<IOutletRegistrySubscriber> _subscribers = new List<IOutletRegistrySubscriber>();

    public void RegisterComponent(IOutletComponent component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        if (_components.Contains(component)) return;

        _components.Add(component);

        foreach (var subscriber in _subscribers) subscriber.OnComponentRegistered(component);
    }

    public void UnregisterComponent(IOutletComponent component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        _components.Remove(component);

        foreach (var subscriber in _subscribers) subscriber.OnComponentUnregistered(component);
    }

    public void Subscribe(IOutletRegistrySubscriber subscriber)
    {
        if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));

        if (!_subscribers.Contains(subscriber)) _subscribers.Add(subscriber);
    }

    public void Unsubscribe(IOutletRegistrySubscriber subscriber)
    {
        if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));

        _subscribers.Remove(subscriber);
    }
}
