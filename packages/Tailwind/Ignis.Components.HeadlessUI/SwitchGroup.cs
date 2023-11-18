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
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<CascadingValue<SwitchGroup>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<SwitchGroup>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<SwitchGroup>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<SwitchGroup>.ChildContent),
                this.GetChildContent<SwitchGroup, SwitchGroup>(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void SetSwitch(Switch @switch)
    {
        _switch = @switch ?? throw new ArgumentNullException(nameof(@switch));
    }

    /// <inheritdoc />
    public void SetLabel(SwitchLabel label)
    {
        Label = label ?? throw new ArgumentNullException(nameof(label));
    }

    /// <inheritdoc />
    public void SetDescription(SwitchDescription description)
    {
        Description = description ?? throw new ArgumentNullException(nameof(description));
    }

    /// <inheritdoc />
    public void ToggleSwitch()
    {
        _switch?.Toggle();
    }
}
