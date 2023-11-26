using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TransitionChild : TransitionBase<TransitionChild>, IDisposable
{
    [CascadingParameter] public Transition Parent { get; set; } = null!;

    [Parameter] public RenderFragment<TransitionChild>? ChildContent { get; set; }

    public TransitionChild() : base("div")
    {
        SetAttributes(ArraySegment<Func<KeyValuePair<string, object?>>>.Empty);
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
        if (!RenderContent) return;

        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }

    public void Hide(Action? continueWith = null)
    {
        LeaveTransition(continueWith);
    }

    public void Show(Action? continueWith = null)
    {
        EnterTransition(continueWith);
    }

    public void Dispose()
    {
        Parent.RemoveChild(this);
    }
}
