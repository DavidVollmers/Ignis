using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class PopoverPanel : DynamicComponentBase<PopoverPanel>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter] public Popover Popover { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public PopoverPanel() : base("div")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Popover.GetId(this)),
            () => new KeyValuePair<string, object?>("tabindex", -1),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Popover == null)
        {
            throw new InvalidOperationException(
                $"{nameof(PopoverPanel)} must be used inside a {nameof(HeadlessUI.Popover)}.");
        }

        Popover.Controlled = this;
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!Popover.IsOpen) return;

        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor(3, this, ChildContent);
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }
}
