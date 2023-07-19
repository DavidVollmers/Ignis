namespace Ignis.Components.HeadlessUI;

public interface ITabGroup
{
    int DefaultIndex { get; set; }
    
    int SelectedIndex { get; set; }

    internal bool IsTabSelected(ITab tab);
    
    internal void AddTab(ITab tab);

    internal void RemoveTab(ITab tab);
}
