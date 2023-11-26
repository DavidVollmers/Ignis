using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class SwitchGroup : DynamicComponentBase<SwitchGroup>, IAriaCheckGroup
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc />
    public IAriaComponentPart? Label { get; set; }

    public Switch? Switch { get; set; }

    public IAriaComponentPart? Description { get; set; }

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-switch-" + Guid.NewGuid().ToString("N");

    public SwitchGroup() : base(typeof(Fragment))
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", GetId(this)),
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

    /// <inheritdoc />
    public string? GetId(IAriaComponentPart? componentPart)
    {
        if (componentPart == null) return null;

        if (componentPart.Id != null) return componentPart.Id;

        if (componentPart == Label) return Id + "-label";

        if (componentPart == Switch) return Id + "-button";

        return null;
    }

    public void ToggleSwitch()
    {
        Switch?.Toggle();
    }
}
