using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components;

public sealed class Dynamic : IgnisRigidComponentBase, IDynamicParentComponent
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

    [Parameter] public RenderFragment<IDynamicComponent>? _ { get; set; }
    
    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<IDynamicComponent, Dynamic>(2, this, ChildContent);

        builder.CloseAs(this);
    }
}
