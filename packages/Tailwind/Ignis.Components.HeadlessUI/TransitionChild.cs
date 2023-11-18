using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TransitionChild : TransitionBase, IDynamicParentComponent<TransitionChild>, IDisposable
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

    [CascadingParameter] public Transition Parent { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<TransitionChild>? _ { get; set; }

    [Parameter] public RenderFragment<TransitionChild>? ChildContent { get; set; }

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    public TransitionChild()
    {
        AsElement = "div";
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Parent == null)
        {
            throw new InvalidOperationException(
                $"{nameof(TransitionChild)} must be used inside a {nameof(Transition)}.");
        }

        Parent.AddChild(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (RenderContent)
            builder.AddChildContentFor(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void Hide(Action? continueWith = null)
    {
        LeaveTransition(continueWith);
    }

    /// <inheritdoc />
    public void Show(Action? continueWith = null)
    {
        EnterTransition(continueWith);
    }

    public void Dispose()
    {
        Parent.RemoveChild(this);
    }
}
