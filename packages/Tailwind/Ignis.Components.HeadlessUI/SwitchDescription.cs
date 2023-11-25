using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class SwitchDescription : DynamicComponentBase<SwitchDescription>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter] public SwitchGroup SwitchGroup { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public SwitchDescription() : base("p")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", SwitchGroup.GetId(SwitchGroup.Description)),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (SwitchGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(SwitchDescription)} must be used inside a {nameof(HeadlessUI.SwitchGroup)}.");
        }

        SwitchGroup.Description = this;
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
