namespace Ignis.Components;

public interface IOutletRegistry
{
    void RegisterComponent(IOutletComponent component);

    void UnregisterComponent(IOutletComponent component);
    
    void Subscribe(IOutletRegistrySubscriber subscriber);
    
    void Unsubscribe(IOutletRegistrySubscriber subscriber);
}
