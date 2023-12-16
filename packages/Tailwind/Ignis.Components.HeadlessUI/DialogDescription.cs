using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

/// <summary>
/// The description of a dialog.
/// </summary>
public sealed class DialogDescription : DynamicComponentBase<DialogDescription>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter] public string? Id { get; set; }

    [CascadingParameter] public Dialog Dialog { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DialogDescription"/> class.
    /// </summary>
    public DialogDescription() : base("p")
    {
        SetAttributes(new[] { () => new KeyValuePair<string, object?>("id", Dialog.GetId(this)), });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Dialog == null)
        {
            throw new InvalidOperationException(
                $"{nameof(DialogDescription)} must be used inside a {nameof(HeadlessUI.Dialog)}.");
        }

        Dialog.Description = this;
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
