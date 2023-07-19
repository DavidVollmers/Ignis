using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TabGroup : IgnisComponentBase, ITabGroup
{
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
    [Parameter] public RenderFragment<ITabGroup>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?>? Attributes => AdditionalAttributes;

    public TabGroup()
    {
        AsComponent = typeof(Fragment);
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (SelectedIndex == DefaultIndex) return;
        
        SelectedIndex = DefaultIndex;
        SelectedIndexChanged.InvokeAsync(DefaultIndex);
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
                ChildContent?.Invoke(this));

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
        
        SelectedIndex = index;
        SelectedIndexChanged.InvokeAsync(index);

        ForceUpdate();
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
}
