namespace Ignis.Components;

public interface IContentRegistry
{
    void RegisterContentProvider(IContentProvider provider);

    void UnregisterContentProvider(IContentProvider provider);

    void Subscribe(IContentRegistrySubscriber subscriber);

    void Unsubscribe(IContentRegistrySubscriber subscriber);
}
