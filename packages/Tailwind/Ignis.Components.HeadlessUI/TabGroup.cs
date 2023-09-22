using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TabGroup : IgnisComponentBase, ITabGroup
{
    private readonly IList<ITabPanel> _tabPanels = new List<ITabPanel>();
    private readonly IList<ITab> _tabs = new List<ITab>();

    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    [Parameter]
    public string? AsElement
    {
        get => _asElement;
        set
        {
            _asElement = value;
            _asComponent = null;
        }
    }

    /// <inheritdoc />
    [Parameter]
    public Type? AsComponent
    {
        get => _asComponent;
        set
        {
            _asComponent = value;
            _asElement = null;
        }
    }

    /// <inheritdoc />
    [Parameter]
    public int DefaultIndex { get; set; }

    /// <inheritdoc />
    [Parameter]
    public int SelectedIndex { get; set; }

    [Parameter] public EventCallback<int> SelectedIndexChanged { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<ITabGroup>? _ { get; set; }

    [Parameter] public RenderFragment<ITabGroup>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public ITab[] Tabs => _tabs.ToArray();

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    public TabGroup()
    {
        AsComponent = typeof(Fragment);
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
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, AdditionalAttributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<CascadingValue<ITabGroup>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<ITabGroup>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<ITabGroup>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<ITabGroup>.ChildContent),
                this.GetChildContent(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public bool IsTabSelected(ITab tab)
    {
        if (tab == null) throw new ArgumentNullException(nameof(tab));

        var index = _tabs.IndexOf(tab);

        return index == SelectedIndex;
    }

    /// <inheritdoc />
    public void SelectTab(ITab tab)
    {
        if (tab == null) throw new ArgumentNullException(nameof(tab));

        var index = _tabs.IndexOf(tab);

        if (index == -1) return;

        var __ = SelectedIndexChanged.InvokeAsync(SelectedIndex = index);

        var ___ = tab.FocusAsync();

        Update();
    }

    /// <inheritdoc />
    public void AddTab(ITab tab)
    {
        if (tab == null) throw new ArgumentNullException(nameof(tab));

        if (!_tabs.Contains(tab)) _tabs.Add(tab);
    }

    /// <inheritdoc />
    public void RemoveTab(ITab tab)
    {
        if (tab == null) throw new ArgumentNullException(nameof(tab));

        _tabs.Remove(tab);
    }

    /// <inheritdoc />
    public bool IsTabPanelSelected(ITabPanel tabPanel)
    {
        if (tabPanel == null) throw new ArgumentNullException(nameof(tabPanel));

        var index = _tabPanels.IndexOf(tabPanel);

        return index == SelectedIndex;
    }

    /// <inheritdoc />
    public void AddTabPanel(ITabPanel tabPanel)
    {
        if (tabPanel == null) throw new ArgumentNullException(nameof(tabPanel));

        if (!_tabPanels.Contains(tabPanel)) _tabPanels.Add(tabPanel);
    }

    /// <inheritdoc />
    public void RemoveTabPanel(ITabPanel tabPanel)
    {
        if (tabPanel == null) throw new ArgumentNullException(nameof(tabPanel));

        _tabPanels.Remove(tabPanel);
    }
}
