using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class SwitchGroup : DynamicComponentBase<SwitchGroup>
{
    private Switch? _switch;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public SwitchLabel? Label { get; private set; }

    public SwitchDescription? Description { get; private set; }

    public string Id { get; } = "ignis-hui-switch-" + Guid.NewGuid().ToString("N");

    public SwitchGroup() : base(typeof(Fragment))
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id),
        });
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
