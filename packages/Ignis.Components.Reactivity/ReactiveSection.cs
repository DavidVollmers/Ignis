using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.Reactivity;

public sealed class ReactiveSection : IgnisComponentBase, IDynamicParentComponent<ReactiveSection>, IDisposable
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

    [Parameter, EditorRequired] public ReactiveExpression For { get; set; } = null!;

    [Parameter] public RenderFragment<ReactiveSection>? _ { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    public ReactiveSection()
    {
        AsComponent = typeof(Fragment);
    }

    protected override void OnUpdate()
    {
        For.Subscribe(this);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor(2, this, ChildContent);

        builder.CloseAs(this);
    }

    public void Dispose()
    {
        For.Unsubscribe(this);
    }
}
