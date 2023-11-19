using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components;

public sealed class Dynamic : DynamicComponentBase<Dynamic>
{
    [Parameter] public RenderFragment? ChildContent { get; set; }

    public Dynamic() : base(typeof(Fragment))
    {
        SetAttributes(ArraySegment<Func<KeyValuePair<string, object?>>>.Empty);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor(2, this, ChildContent);

        builder.CloseAs(this);
    }
}
