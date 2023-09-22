using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TabPanels : IgnisRigidComponentBase, IDynamicParentComponent
{
    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    [Parameter]
    public string? AsElement
    {
        get => _asElement;
        set
        {
            _asElement = value;
            _asComponent = null;
        }
    }

    /// <inheritdoc />
    [Parameter]
    public Type? AsComponent
    {
        get => _asComponent;
        set
        {
            _asComponent = value;
            _asElement = null;
        }
    }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IDynamicComponent>? _ { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    public TabPanels()
    {
        AsElement = "div";
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<IDynamicComponent, TabPanels>(2, this, ChildContent);

        builder.CloseAs(this);
    }
}
