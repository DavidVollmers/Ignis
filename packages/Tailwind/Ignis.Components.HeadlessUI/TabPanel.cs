using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TabPanel : IgnisComponentBase, ITabPanel, IDisposable
{
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
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public bool IsSelected => TabGroup.IsTabPanelSelected(this);

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    //TODO aria-labelledby
    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes
    {
        get
        {
            var attributes = new Dictionary<string, object?>
            {
                { "role", "tabpanel" },
                { "tabindex", IsSelected ? 0 : -1 }
            };

            // ReSharper disable once InvertIf
            if (AdditionalAttributes != null)
            {
                foreach (var (key, value) in AdditionalAttributes)
                {
                    attributes[key] = value;
                }
            }
            
            return attributes;
        }
    }

    public TabPanel()
    {
        AsElement = "div";
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
