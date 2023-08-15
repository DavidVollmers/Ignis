using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Popover : OpenCloseWithTransitionComponentBase, IPopover
{
    private IPopoverButton? _button;
    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    protected override ElementReference? TargetElement => Element;

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
    public RenderFragment<IPopover>? _ { get; set; }

    [Parameter] public RenderFragment<IPopover>? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the popover.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-popover-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    public Popover()
    {
        AsElement = "div";
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<CascadingValue<IPopover>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<IPopover>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<IPopover>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<IPopover>.ChildContent), this.GetChildContent(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void SetButton(IPopoverButton button)
    {
        _button = button ?? throw new ArgumentNullException(nameof(button));
    }

    /// <inheritdoc />
    protected override void OnBlur()
    {
        Close();
    }
}
