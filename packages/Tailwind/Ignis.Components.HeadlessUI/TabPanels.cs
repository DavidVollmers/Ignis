using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TabPanels : DynamicComponentBase<TabPanels>
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    public TabPanels() : base("div")
    {
        SetAttributes(ArraySegment<Func<KeyValuePair<string, object?>>>.Empty);
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
