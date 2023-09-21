namespace Ignis.Components;

internal class ContentRegistry : IContentRegistry
{
    private readonly IList<IContentProvider> _components = new List<IContentProvider>();
    private readonly IList<IContentRegistrySubscriber> _subscribers = new List<IContentRegistrySubscriber>();

    public void RegisterContentProvider(IContentProvider provider)
    {
        if (provider == null) throw new ArgumentNullException(nameof(provider));

        if (_components.Contains(provider)) return;

        _components.Add(provider);

        foreach (var subscriber in _subscribers) subscriber.OnProviderRegistered(provider);
    }

    public void UnregisterContentProvider(IContentProvider provider)
    {
        if (provider == null) throw new ArgumentNullException(nameof(provider));

        _components.Remove(provider);

        foreach (var subscriber in _subscribers) subscriber.OnProviderUnregistered(provider);
    }

    public void Subscribe(IContentRegistrySubscriber subscriber)
    {
        if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));

        if (_subscribers.Contains(subscriber)) return;

        _subscribers.Add(subscriber);

        foreach (var component in _components) subscriber.OnProviderRegistered(component);
    }

    public void Unsubscribe(IContentRegistrySubscriber subscriber)
    {
        if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));

        _subscribers.Remove(subscriber);
    }
}
