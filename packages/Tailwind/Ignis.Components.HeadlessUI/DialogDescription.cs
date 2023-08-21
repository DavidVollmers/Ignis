using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class DialogDescription : IgnisRigidComponentBase, IDialogDescription
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

    [CascadingParameter] public IDialog Dialog { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IDialogDescription>? _ { get; set; }

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
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    public DialogDescription()
    {
        AsElement = "p";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id ?? Dialog.Id + "-description"),
        });
    }

    /// <inheritdoc />
    protected override void OnRender()
    {
        if (Dialog == null)
        {
            throw new InvalidOperationException(
                $"{nameof(DialogDescription)} must be used inside a {nameof(HeadlessUI.Dialog)}.");
        }

        Dialog.SetDescription(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor<IDialogDescription, DialogDescription>(3, this, ChildContent);
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }
}
