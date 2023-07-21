using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TransitionChild : TransitionBase, ITransitionChild
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
    public RenderFragment<ITransitionChild>? _ { get; set; }

    [Parameter] public RenderFragment<ITransitionChild>? ChildContent { get; set; }

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    public TransitionChild()
    {
        AsElement = "div";
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<ITransitionChild, TransitionChild>(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }
}
