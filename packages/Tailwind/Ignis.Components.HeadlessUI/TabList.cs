using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TabList : DynamicComponentBase<TabList>
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    public TabList() : base("div")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("role", "tablist"),
            () => new KeyValuePair<string, object?>("aria-orientation", "horizontal"),
        });
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, AdditionalAttributes!);
        builder.AddChildContentFor(2, this, ChildContent);

        builder.CloseAs(this);
    }
}
