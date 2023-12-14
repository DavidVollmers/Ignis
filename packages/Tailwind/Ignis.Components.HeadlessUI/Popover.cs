using Ignis.Components.HeadlessUI.Aria;
using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class Popover : OpenCloseWithTransitionComponentBase, IDynamicParentComponent<Popover>, IAriaControl
{
    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    protected override IEnumerable<object> Targets
    {
        get
        {
            if (Button != null) yield return Button;

            if (Controlled != null) yield return Controlled;
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<string> KeysToCapture { get; } = new[] { "Escape" };

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
    public RenderFragment<Popover>? _ { get; set; }

    [Parameter] public RenderFragment<Popover>? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the popover.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    public string Id { get; } = "ignis-hui-popover-" + Guid.NewGuid().ToString("N");

    public PopoverButton? Button { get; set; }

    public IAriaComponentPart? Controlled { get; set; }

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
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
        builder.OpenComponent<CascadingValue<Popover>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<Popover>.IsFixed), value: true);
        builder.AddAttribute(2, nameof(CascadingValue<Popover>.Value), this);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddAttribute(3, nameof(CascadingValue<Popover>.ChildContent), (RenderFragment)(builder =>
        {
            builder.OpenAs(4, this);
            builder.AddMultipleAttributes(5, Attributes!);
            builder.AddChildContentFor(6, this, ChildContent);

            builder.CloseAs(this);
        }));

        builder.CloseComponent();
    }

    /// <inheritdoc />
    protected override void OnTargetBlur()
    {
        Close();
    }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        if (!string.Equals(eventArgs.Code, "Escape", StringComparison.Ordinal)) return;

        Close();
    }

    /// <inheritdoc />
    public string? GetId(IAriaComponentPart? componentPart)
    {
        if (componentPart == null) return null;

        if (componentPart.Id != null) return componentPart.Id;

        if (componentPart == Button) return Id + "-button";

        if (componentPart == Controlled) return Id + "-controlled";

        return null;
    }
}
