namespace Ignis.Components;

internal interface IOutletRegistry
{
    void AddOutlet(IOutlet outlet);
    
    void RemoveOutlet(IOutlet outlet);
    
    void RegisterComponent(IOutletComponent component);

    void UnregisterComponent(IOutletComponent component);

    bool HasOutletFor(IOutletComponent component);
}
