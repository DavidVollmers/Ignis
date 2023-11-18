using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class MenuItem : IgnisComponentBase, IDynamicParentComponent<MenuItem>, IDisposable
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

    /// <inheritdoc />
    [Parameter]
    public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public IMenu Menu { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IMenuItem>? _ { get; set; }

    [Parameter] public RenderFragment<IMenuItem>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc />
    public bool IsActive => Menu.ActiveItem == this;

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public MenuItem()
    {
        AsComponent = typeof(Fragment);

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("tabindex", -1),
            () => new KeyValuePair<string, object?>("role", "menuitem"),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)), () =>
                new KeyValuePair<string, object?>("onmouseenter",
                    EventCallback.Factory.Create(this, OnMouseEnter)),
            () => new KeyValuePair<string, object?>("onmouseleave",
                EventCallback.Factory.Create(this, OnMouseLeave))
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Menu == null)
        {
            throw new InvalidOperationException($"{nameof(MenuItem)} must be used inside a {nameof(HeadlessUI.Menu)}.");
        }

        Menu.AddItem(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor<IMenuItem, MenuItem>(3, this, ChildContent?.Invoke(this));
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void Click()
    {
        var @event = new ComponentEvent();

        var __ = OnClick.InvokeAsync(@event);

        if (@event.CancellationToken.IsCancellationRequested) return;

        Menu.Close();
    }

    private void OnMouseEnter()
    {
        Menu.SetItemActive(this, true);
    }

    private void OnMouseLeave()
    {
        Menu.SetItemActive(this, false);
    }

    public void Dispose()
    {
        Menu.RemoveItem(this);
    }
}
