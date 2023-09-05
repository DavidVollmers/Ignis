namespace Ignis.Components;

internal class ContentRegistry : IContentRegistry
{
    private readonly IList<IContentProvider> _components = new List<IContentProvider>();
    private readonly IList<IContentRegistrySubscriber> _subscribers = new List<IContentRegistrySubscriber>();

    public void RegisterContentProvider(IContentProvider component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        if (_components.Contains(component)) return;

        _components.Add(component);

        foreach (var subscriber in _subscribers) subscriber.OnProviderRegistered(component);
    }

    public void UnregisterContentProvider(IContentProvider component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        _components.Remove(component);

        foreach (var subscriber in _subscribers) subscriber.OnProviderUnregistered(component);
    }

    public void Subscribe(IContentRegistrySubscriber subscriber)
    {
        if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));

        if (!_subscribers.Contains(subscriber)) _subscribers.Add(subscriber);
    }

    public void Unsubscribe(IContentRegistrySubscriber subscriber)
    {
        if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));

        _subscribers.Remove(subscriber);
    }
}
