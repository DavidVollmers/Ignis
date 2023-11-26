using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class SwitchLabel : DynamicComponentBase<SwitchLabel>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter] public SwitchGroup SwitchGroup { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public SwitchLabel() : base("label")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", SwitchGroup.GetId(SwitchGroup.Label)), () =>
                new KeyValuePair<string, object?>("onclick",
                    EventCallback.Factory.Create(this, SwitchGroup.ToggleSwitch)),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (SwitchGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(SwitchLabel)} must be used inside a {nameof(HeadlessUI.SwitchGroup)}.");
        }

        SwitchGroup.Label = this;
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor(2, this, ChildContent);

        builder.CloseAs(this);
    }
}
