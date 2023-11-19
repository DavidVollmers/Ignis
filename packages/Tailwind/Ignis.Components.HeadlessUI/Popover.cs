using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class Popover : OpenCloseWithTransitionComponentBase, IDynamicParentComponent<Popover>
{
    private PopoverPanel? _panel;
    private PopoverButton? _button;
    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    protected override IEnumerable<object> Targets
    {
        get
        {
            if (_button != null) yield return _button;

            if (_panel != null) yield return _panel;
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

    public void SetButton(PopoverButton button)
    {
        _button = button ?? throw new ArgumentNullException(nameof(button));
    }

    public void SetPanel(PopoverPanel panel)
    {
        _panel = panel ?? throw new ArgumentNullException(nameof(panel));
    }

    /// <inheritdoc />
    protected override void OnBlur()
    {
        Close();
    }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        if (!string.Equals(eventArgs.Code, "Escape", StringComparison.Ordinal)) return;

        Close();
    }
}
