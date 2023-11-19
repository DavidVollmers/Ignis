using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components;

public sealed class Fragment : IgnisComponentBase
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    // not used but required to catch all unmatched parameters to not throw an exception
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? UnmatchedParameters { private get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.AddContent(0, ChildContent);
    }
}
