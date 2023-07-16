using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxLabel : IgnisStaticComponentBase, IDynamicComponent
{
    [Parameter] public string? AsElement { get; set; } = "label";

    [Parameter] public Type? AsComponent { get; set; }
    
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? Attributes { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        
        builder.AddChildContentFor(2, this, ChildContent);
        
        builder.CloseAs(this);
    }
}
