namespace Ignis.Components.HeadlessUI;

public interface ITabPanel : IDynamicParentComponent<ITabPanel>
{
    bool IsSelected { get; }
}
