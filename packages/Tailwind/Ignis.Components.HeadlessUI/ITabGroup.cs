namespace Ignis.Components.HeadlessUI;

public interface ITabGroup : IDynamicParentComponent<ITabGroup>
{
    internal ITab[] Tabs { get; }

    int DefaultIndex { get; set; }

    int SelectedIndex { get; set; }

    internal bool IsTabSelected(ITab tab);

    internal void SelectTab(ITab tab);

    internal void AddTab(ITab tab);

    internal void RemoveTab(ITab tab);

    internal bool IsTabPanelSelected(ITabPanel tabPanel);

    internal void AddTabPanel(ITabPanel tabPanel);

    internal void RemoveTabPanel(ITabPanel tabPanel);
}
