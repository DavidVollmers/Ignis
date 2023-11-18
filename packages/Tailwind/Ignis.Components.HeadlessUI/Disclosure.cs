using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Disclosure : OpenCloseWithTransitionComponentBase, IDynamicParentComponent<Disclosure>
{
    private DisclosureButton? _button;
    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    protected override IEnumerable<object> Targets
    {
        get
        {
            if (_button != null) yield return _button;

            if (Panel != null) yield return Panel;
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
    public RenderFragment<Disclosure>? _ { get; set; }

    [Parameter] public RenderFragment<Disclosure>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    public DisclosurePanel? Panel { get; private set; }

    public string Id { get; } = "ignis-hui-disclosure-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
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
            builder.OpenComponent<CascadingValue<Disclosure>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<Disclosure>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<Disclosure>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<Disclosure>.ChildContent),
                this.GetChildContent(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    public void SetPanel(DisclosurePanel panel)
    {
        Panel = panel ?? throw new ArgumentNullException(nameof(panel));
    }

    public void SetButton(DisclosureButton button)
    {
        _button = button ?? throw new ArgumentNullException(nameof(button));
    }
}
