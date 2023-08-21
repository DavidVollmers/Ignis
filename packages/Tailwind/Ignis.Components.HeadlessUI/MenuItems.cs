using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class MenuItems : IgnisRigidComponentBase, IDynamicParentComponent
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

    [CascadingParameter] public IMenu Menu { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IDynamicComponent>? _ { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc cref="IDynamicParentComponent{T}.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public MenuItems()
    {
        AsElement = "div";

        //TODO aria-active-descendant
        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("tabindex", -1),
            () => new KeyValuePair<string, object?>("role", "menu"), () => new KeyValuePair<string, object?>(
                "aria-labelledby", Menu.Button == null ? null : Menu.Button.Id ?? Menu.Id + "-button")
        });
    }

    /// <inheritdoc />
    protected override void OnRender()
    {
        if (Menu == null)
        {
            throw new InvalidOperationException(
                $"{nameof(MenuItems)} must be used inside a {nameof(HeadlessUI.Menu)}.");
        }

        Menu.SetItems(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!Menu.IsOpen) return;

        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor<IDynamicComponent, MenuItems>(3, this, ChildContent);
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }
}
