using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxLabel : IgnisRigidComponentBase, IListboxLabel
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
    public string? Id { get; set; }

    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IListboxLabel>? _ { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public ListboxLabel()
    {
        AsElement = "label";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id ?? Listbox.Id + "-label"), () =>
                new KeyValuePair<string, object?>("onclick",
                    EventCallback.Factory.Create(this, Listbox.FocusAsync))
        });
    }

    /// <inheritdoc />
    protected override void OnRender()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException(
                $"{nameof(ListboxLabel)} must be used inside a {nameof(Listbox<object>)}.");
        }

        Listbox.SetLabel(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor<IListboxLabel, ListboxLabel>(3, this, ChildContent);

        builder.CloseAs(this);
    }
}
