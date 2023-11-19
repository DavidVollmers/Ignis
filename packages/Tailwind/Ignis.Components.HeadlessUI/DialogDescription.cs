using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class DialogDescription : IgnisDynamicComponentBase<DialogDescription>
{
    [Parameter] public string? Id { get; set; }

    [CascadingParameter] public Dialog Dialog { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public DialogDescription() : base("p")
    {
        SetAttributes(new[] { () => new KeyValuePair<string, object?>("id", Id ?? Dialog.Id + "-description"), });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Dialog == null)
        {
            throw new InvalidOperationException(
                $"{nameof(DialogDescription)} must be used inside a {nameof(HeadlessUI.Dialog)}.");
        }

        Dialog.SetDescription(this);
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
