using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components;

public sealed class Fragment : IgnisRigidComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    
    // not used but required to catch all unmatched attributes to not throw an exception
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.AddContent(0, ChildContent);
    }
}
