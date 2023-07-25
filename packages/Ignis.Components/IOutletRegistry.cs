namespace Ignis.Components;

internal interface IOutletRegistry
{
    void RegisterComponent(IOutletComponent component);

    void UnregisterComponent(IOutletComponent component);

    bool HasOutletFor(IOutletComponent component);
}
