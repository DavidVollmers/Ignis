using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Disclosure : IgnisComponentBase, IDynamicComponent, IOpenClose
{
    private Type? _asComponent;
    private string? _asElement;

    [Parameter]
#pragma warning disable BL0007
    public string? AsElement
#pragma warning restore BL0007
    {
        get => _asElement;
        set
        {
            _asElement = value;
            _asComponent = null;
        }
    }

    [Parameter]
#pragma warning disable BL0007
    public Type? AsComponent
#pragma warning restore BL0007
    {
        get => _asComponent;
        set
        {
            _asComponent = value;
            _asElement = null;
        }
    }
    
    public bool IsOpen { get; private set; }

    [Parameter] public RenderFragment<IOpenClose>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }
    
    public Disclosure()
    {
        AsComponent = typeof(Fragment);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, AdditionalAttributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
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
