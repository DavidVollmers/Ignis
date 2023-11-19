using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class SwitchGroup : IgnisRigidComponentBase, IDynamicParentComponent<SwitchGroup>
{
    private Type? _asComponent;
    private string? _asElement;
    private Switch? _switch;

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
    public RenderFragment<SwitchGroup>? _ { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the switch group.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public SwitchLabel? Label { get; private set; }

    /// <inheritdoc />
    public SwitchDescription? Description { get; private set; }

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-switch-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    public SwitchGroup()
    {
        AsComponent = typeof(Fragment);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<SwitchGroup>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<SwitchGroup>.IsFixed), value: true);
        builder.AddAttribute(2, nameof(CascadingValue<SwitchGroup>.Value), this);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddAttribute(3, nameof(CascadingValue<SwitchGroup>.ChildContent), (RenderFragment)(builder =>
        {
            builder.OpenAs(4, this);
            builder.AddMultipleAttributes(5, Attributes!);
            builder.AddChildContentFor(6, this, ChildContent);

            builder.CloseAs(this);
        }));

        builder.CloseComponent();
    }

    public void SetSwitch(Switch @switch)
    {
        _switch = @switch ?? throw new ArgumentNullException(nameof(@switch));
    }

    public void SetLabel(SwitchLabel label)
    {
        Label = label ?? throw new ArgumentNullException(nameof(label));
    }

    public void SetDescription(SwitchDescription description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    public void ToggleSwitch()
    {
        _switch?.Toggle();
    }
}
