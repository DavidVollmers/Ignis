using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class DialogTitle : IgnisRigidComponentBase, IDialogTitle
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
    [Parameter] public string? Id { get; set; }
    
    [CascadingParameter] public IDialog Dialog { get; set; } = null!;
    
    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IDialogTitle>? _ { get; set; }

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
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    public DialogTitle()
    {
        AsElement = "h2";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id ?? Dialog.Id + "-label"),
        });
    }

    /// <inheritdoc />
    protected override void OnRender()
    {
        if (Dialog == null)
        {
            throw new InvalidOperationException(
                $"{nameof(DialogTitle)} must be used inside a {nameof(HeadlessUI.Dialog)}.");
        }
        
        Dialog.SetTitle(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor<IDialogTitle, DialogTitle>(3, this, ChildContent);
        
        builder.CloseAs(this);
    }
}
