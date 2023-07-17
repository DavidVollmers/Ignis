using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components;

public sealed class Dynamic : IgnisDynamicComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, AdditionalAttributes!);
        builder.AddContentFor(2, this, ChildContent);

        builder.CloseAs(this);
    }
}
