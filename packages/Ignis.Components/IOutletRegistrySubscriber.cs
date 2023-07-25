namespace Ignis.Components;

public interface IOutletRegistrySubscriber
{
    void OnComponentRegistered(IOutletComponent component);
    
    void OnComponentUnregistered(IOutletComponent component);
}
