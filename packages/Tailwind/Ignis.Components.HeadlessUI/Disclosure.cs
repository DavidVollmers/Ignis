using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Disclosure : IgnisComponentBase, IDynamicComponent
{
    [Parameter] public string? AsElement { get; set; }

    [Parameter] public Type? AsComponent { get; set; } = typeof(Fragment);

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        
        
        
        builder.CloseAs(this);
    }
}
