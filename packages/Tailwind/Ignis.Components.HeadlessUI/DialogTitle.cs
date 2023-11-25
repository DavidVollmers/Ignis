using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class DialogTitle : DynamicComponentBase<DialogTitle>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter] public string? Id { get; set; }

    [CascadingParameter] public Dialog Dialog { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public DialogTitle() : base("h2")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Dialog.GetId(this)),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Dialog == null)
        {
            throw new InvalidOperationException(
                $"{nameof(DialogTitle)} must be used inside a {nameof(HeadlessUI.Dialog)}.");
        }

        Dialog.Label = this;
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor(3, this, ChildContent);
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }
}
