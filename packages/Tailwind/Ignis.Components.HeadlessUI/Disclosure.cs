using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Disclosure : IgnisDynamicComponentBase, IOpenClose
{
    public bool IsOpen { get; private set; }

    [Parameter] public RenderFragment<IOpenClose>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? Attributes { get; set; }
    
    public Disclosure()
    {
        AsComponent = typeof(Fragment);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddChildContentFor(2, this, builder =>
        {
            builder.OpenComponent<CascadingValue<IOpenClose>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<IOpenClose>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<IOpenClose>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<IOpenClose>.ChildContent), ChildContent?.Invoke(this));

            builder.CloseComponent();
        });

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
