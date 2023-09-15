namespace Ignis.Components;

public interface IContentRegistrySubscriber
{
    void OnProviderRegistered(IContentProvider provider);

    void OnProviderUnregistered(IContentProvider provider);
}
