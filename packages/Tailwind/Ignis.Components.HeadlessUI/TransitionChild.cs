using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TransitionChild : TransitionBase, ITransitionChild, IDisposable
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

    [CascadingParameter] public ITransition Parent { get; set; }

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
    protected override void OnInitialized()
    {
        if (Parent == null)
        {
            throw new InvalidOperationException(
                $"{nameof(TransitionChild)} must be used inside a {nameof(Transition)}.");
        }

        //TODO check if parent is shown initially (for appear prop)
        Parent.AddChild(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (IsShowing)
            builder.AddChildContentFor<ITransitionChild, TransitionChild>(2, this, ChildContent?.Invoke(this));

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
