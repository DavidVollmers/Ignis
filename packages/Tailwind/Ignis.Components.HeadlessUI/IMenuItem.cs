namespace Ignis.Components.HeadlessUI;

public interface IMenuItem : IDynamicParentComponent<IMenuItem>
{
    bool IsActive { get; }
}
