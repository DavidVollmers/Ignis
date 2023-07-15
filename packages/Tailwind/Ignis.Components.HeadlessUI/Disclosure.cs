using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Disclosure : IgnisComponentBase, IDynamicComponent
{
    public bool IsOpen { get; private set; }
    
    [Parameter] public string? AsElement { get; set; }

    [Parameter] public Type? AsComponent { get; set; } = typeof(Fragment);

    [Parameter] public RenderFragment<Disclosure>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? Attributes { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        
        builder.OpenComponent<CascadingValue<Disclosure>>(2);
        builder.AddAttribute(3, nameof(CascadingValue<Disclosure>.IsFixed), true);
        builder.AddAttribute(4, nameof(CascadingValue<Disclosure>.Value), this);
        builder.AddAttribute(5, nameof(CascadingValue<Disclosure>.ChildContent), ChildContent?.Invoke(this));

        builder.CloseComponent();
        
        builder.CloseAs(this);
    }

    public void Open()
    {
        if (IsOpen) return;
        
        IsOpen = true;
        
        ForceUpdate();
    }

    public void Close()
    {
        if (!IsOpen) return;
        
        IsOpen = false;
        
        ForceUpdate();
    }
}
