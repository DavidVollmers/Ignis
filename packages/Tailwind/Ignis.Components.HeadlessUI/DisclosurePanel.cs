using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class DisclosurePanel : DynamicComponentBase<DisclosurePanel>, IAriaComponentPart
{
    [Parameter] public string? Id { get; set; }

    [CascadingParameter] public Disclosure Disclosure { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public DisclosurePanel() : base("div")
    {
        SetAttributes(new[] { () => new KeyValuePair<string, object?>("id", Disclosure.GetId(this)), });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Disclosure == null)
        {
            throw new InvalidOperationException(
                $"{nameof(DisclosurePanel)} must be used inside a {nameof(HeadlessUI.Disclosure)}.");
        }

        Disclosure.Controlled = this;
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!Disclosure.IsOpen) return;

        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor(3, this, ChildContent);
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }
}
