namespace Ignis.Components.HeadlessUI;

public interface IListboxOption : IDynamicParentComponent<IListboxOption>
{
    bool IsActive { get; }
    
    bool IsSelected { get; }

    internal void Select();
}
