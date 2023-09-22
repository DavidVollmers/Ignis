using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.Reactivity;

public sealed class ReactiveSection<T> : IgnisComponentBase, IDynamicParentComponent, IDisposable
{
    private Type? _asComponent;
    private string? _asElement;

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

    [Parameter, EditorRequired] public ReactiveValue<T> Value { get; set; } = null!;

    [Parameter] public RenderFragment<IDynamicComponent>? _ { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    protected override void OnUpdate()
    {
        Value.Adopt(this);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<IDynamicComponent, ReactiveSection<T>>(2, this, ChildContent);

        builder.CloseAs(this);
    }

    public void Dispose()
    {
        Value.SetFree(this);
    }
}
