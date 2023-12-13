using Ignis.Components.HeadlessUI.Aria;
using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class Tab : FocusComponentBase, IDynamicParentComponent<Tab>, IAriaComponentPart
{
    private readonly AttributeCollection _attributes;

    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    /// <inheritdoc />
    protected override IEnumerable<object> Targets
    {
        get
        {
            yield return this;
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<string> KeysToCapture { get; } = new[] { "ArrowLeft", "ArrowRight" };

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

    [Parameter] public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public TabGroup TabGroup { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<Tab>? _ { get; set; }

    [Parameter] public RenderFragment<Tab>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    public bool IsSelected => TabGroup.IsTabSelected(this);

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public Tab()
    {
        AsElement = "button";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", TabGroup.GetId(this)),
            () => new KeyValuePair<string, object?>("role", "tab"),
            () => new KeyValuePair<string, object?>("aria-selected", IsSelected.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("tabindex", IsSelected ? 0 : -1),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)), () =>
                new KeyValuePair<string, object?>("type",
                    string.Equals(AsElement, "button", StringComparison.OrdinalIgnoreCase) ? "button" : null),
            () =>
            {
                var tabPanel = TabGroup.TabPanels.ElementAtOrDefault(Array.IndexOf(TabGroup.Tabs.ToArray(), this));
                return new KeyValuePair<string, object?>("aria-controls", TabGroup.GetId(tabPanel));
            },
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (TabGroup == null)
        {
            throw new InvalidOperationException($"{nameof(Tab)} must be used inside a {nameof(HeadlessUI.TabGroup)}.");
        }

        TabGroup.AddTab(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor(3, this, ChildContent?.Invoke(this));
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        var tabs = TabGroup.Tabs.ToArray();

        switch (eventArgs.Code)
        {
            case "ArrowLeft":
                (TabGroup.SelectedIndex == 0 ? tabs[^1] : tabs[TabGroup.SelectedIndex - 1]).Click();
                break;
            case "ArrowRight":
                (TabGroup.SelectedIndex == tabs.Length - 1 ? tabs[0] : tabs[TabGroup.SelectedIndex + 1]).Click();
                break;
        }
    }

    public void Click()
    {
        var @event = new ComponentEvent();

        var __ = OnClick.InvokeAsync(@event);

        if (@event.DefaultPrevented) return;

        TabGroup.SelectTab(this);
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            TabGroup.RemoveTab(this);
        }

        base.Dispose(disposing);
    }
}
