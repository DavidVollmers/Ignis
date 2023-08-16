using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Disclosure : OpenCloseWithTransitionComponentBase, IDisclosure
{
    private IDynamicParentComponent? _button;
    private Type? _asComponent;
    private string? _asElement;
    private bool _isOpen;

    /// <inheritdoc />
    protected override IEnumerable<ElementReference> TargetElements
    {
        get
        {
            if (_button?.Element.HasValue == true) yield return _button.Element.Value;

            if (Panel?.Element.HasValue == true) yield return Panel.Element.Value;
        }
    }

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
    public RenderFragment<IDisclosure>? _ { get; set; }

    [Parameter] public RenderFragment<IDisclosure>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public IDisclosurePanel? Panel { get; private set; }

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-disclosure-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    public Disclosure()
    {
        AsComponent = typeof(Fragment);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<CascadingValue<IDisclosure>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<IDisclosure>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<IDisclosure>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<IDisclosure>.ChildContent),
                this.GetChildContent(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void SetPanel(IDisclosurePanel panel)
    {
        Panel = panel ?? throw new ArgumentNullException(nameof(panel));
    }

    /// <inheritdoc />
    public void SetButton(IDynamicParentComponent button)
    {
        _button = button ?? throw new ArgumentNullException(nameof(button));
    }
}
