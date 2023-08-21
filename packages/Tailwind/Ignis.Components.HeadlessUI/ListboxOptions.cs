using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxOptions : IgnisRigidComponentBase, IDynamicParentComponent
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

    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

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

    public ListboxOptions()
    {
        AsElement = "ul";

        //TODO aria-active-descendant
        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("tabindex", -1),
            () => new KeyValuePair<string, object?>("role", "listbox"),
            () => new KeyValuePair<string, object?>("aria-orientation", "vertical"), () =>
                new KeyValuePair<string, object?>("aria-labelledby",
                    Listbox.Button == null ? null : Listbox.Button.Id ?? Listbox.Id + "-button")
        });
    }

    /// <inheritdoc />
    protected override void OnRender()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException(
                $"{nameof(ListboxOptions)} must be used inside a {nameof(Listbox<object>)}.");
        }
        
        Listbox.SetOptions(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!Listbox.IsOpen) return;

        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor<IDynamicComponent, ListboxOptions>(3, this, ChildContent);
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }
}
