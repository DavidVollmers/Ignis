using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TabPanel : IgnisComponentBase, ITabPanel, IDisposable
{
    private readonly AttributeCollection _attributes;

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

    [CascadingParameter] public ITabGroup TabGroup { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<ITabPanel>? _ { get; set; }

    [Parameter] public RenderFragment<ITabPanel>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc />
    public bool IsSelected => TabGroup.IsTabPanelSelected(this);

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public TabPanel()
    {
        AsElement = "div";

        //TODO aria-labelledby
        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("role", "tabpanel"),
            () => new KeyValuePair<string, object?>("tabindex", IsSelected ? 0 : -1)
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (TabGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(TabPanel)} must be used inside a {nameof(HeadlessUI.TabGroup)}.");
        }

        TabGroup.AddTabPanel(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!IsSelected) return;

        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<ITabPanel, TabPanel>(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }

    public void Dispose()
    {
        TabGroup.RemoveTabPanel(this);
    }
}
