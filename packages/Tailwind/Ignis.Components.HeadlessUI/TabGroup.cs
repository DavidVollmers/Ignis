﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TabGroup : DynamicComponentBase<TabGroup>
{
    private readonly IList<TabPanel> _tabPanels = new List<TabPanel>();
    private readonly IList<Tab> _tabs = new List<Tab>();

    [Parameter]
    public int DefaultIndex { get; set; }

    [Parameter]
    public int SelectedIndex { get; set; }

    [Parameter] public EventCallback<int> SelectedIndexChanged { get; set; }

    [Parameter] public RenderFragment<TabGroup>? ChildContent { get; set; }

    public Tab[] Tabs => _tabs.ToArray();

    public TabGroup() : base(typeof(Fragment))
    {
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (SelectedIndex == DefaultIndex) return;

        var __ = SelectedIndexChanged.InvokeAsync(SelectedIndex = DefaultIndex);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<TabGroup>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<TabGroup>.IsFixed), value: true);
        builder.AddAttribute(2, nameof(CascadingValue<TabGroup>.Value), this);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddAttribute(3, nameof(CascadingValue<TabGroup>.ChildContent), (RenderFragment)(builder =>
        {
            builder.OpenAs(4, this);
            builder.AddMultipleAttributes(5, AdditionalAttributes!);
            builder.AddChildContentFor(6, this, ChildContent);

            builder.CloseAs(this);
        }));

        builder.CloseComponent();
    }

    public bool IsTabSelected(Tab tab)
    {
        if (tab == null) throw new ArgumentNullException(nameof(tab));

        var index = _tabs.IndexOf(tab);

        return index == SelectedIndex;
    }

    public void SelectTab(Tab tab)
    {
        if (tab == null) throw new ArgumentNullException(nameof(tab));

        var index = _tabs.IndexOf(tab);

        if (index == -1) return;

        var __ = SelectedIndexChanged.InvokeAsync(SelectedIndex = index);

        var ___ = tab.FocusAsync();

        Update();
    }

    public void AddTab(Tab tab)
    {
        if (tab == null) throw new ArgumentNullException(nameof(tab));

        if (!_tabs.Contains(tab)) _tabs.Add(tab);
    }

    public void RemoveTab(Tab tab)
    {
        if (tab == null) throw new ArgumentNullException(nameof(tab));

        _tabs.Remove(tab);
    }

    public bool IsTabPanelSelected(TabPanel tabPanel)
    {
        if (tabPanel == null) throw new ArgumentNullException(nameof(tabPanel));

        var index = _tabPanels.IndexOf(tabPanel);

        return index == SelectedIndex;
    }

    public void AddTabPanel(TabPanel tabPanel)
    {
        if (tabPanel == null) throw new ArgumentNullException(nameof(tabPanel));

        if (!_tabPanels.Contains(tabPanel)) _tabPanels.Add(tabPanel);
    }

    public void RemoveTabPanel(TabPanel tabPanel)
    {
        if (tabPanel == null) throw new ArgumentNullException(nameof(tabPanel));

        _tabPanels.Remove(tabPanel);
    }
}
