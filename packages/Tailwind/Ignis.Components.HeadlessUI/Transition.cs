using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Transition : TransitionBase, ITransition, IHandleAfterRender
{
    private readonly IList<ITransitionChild> _children = new List<ITransitionChild>();

    private bool _didRenderOnce;
    private bool _showInitially;
    private Type? _asComponent;
    private string? _asElement;
    private bool _show;

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

    [Parameter]
    public bool Show
    {
        get => _show;
        set
        {
            _showInitially = _show = value;

            if (!_didRenderOnce) return;

            if (_show) EnterTransition();
            else LeaveTransition();
        }
    }

    [Parameter] public bool Appear { get; set; }

    [CascadingParameter] public IListbox? Listbox { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<ITransition>? _ { get; set; }

    [Parameter] public RenderFragment<ITransition>? ChildContent { get; set; }

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    public Transition()
    {
        AsElement = "div";
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Listbox?.SetTransition(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<CascadingValue<ITransition>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<ITransition>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<ITransition>.Value), this);
            if (IsShowing)
                builder.AddAttribute(6, nameof(CascadingValue<ITransition>.ChildContent),
                    this.GetChildContent(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void Hide(Action? continueWith = null)
    {
        LeaveTransition(continueWith);
    }

    void ITransition.Show(Action? continueWith)
    {
        EnterTransition(continueWith);
    }

    /// <inheritdoc />
    protected override void EnterTransition(Action? continueWith = null)
    {
        var childCount = _children.Count;

        void InternalContinueWith()
        {
            if (_children.Count == childCount)
            {
                continueWith?.Invoke();
                return;
            }

            foreach (var child in _children)
            {
                child.Show(InternalContinueWith);
            }

            childCount = _children.Count;
        }

        base.EnterTransition(InternalContinueWith);
    }

    protected override void LeaveTransition(Action? continueWith = null)
    {
        var childCount = _children.Count;

        void InternalContinueWith()
        {
            if (_children.Count == childCount)
            {
                continueWith?.Invoke();
                return;
            }

            foreach (var child in _children)
            {
                child.Hide(InternalContinueWith);
            }

            childCount = _children.Count;
        }

        base.LeaveTransition(InternalContinueWith);
    }

    /// <inheritdoc />
    public void AddChild(ITransitionChild child)
    {
        if (child == null) throw new ArgumentNullException(nameof(child));

        if (!_children.Contains(child)) _children.Add(child);
    }

    /// <inheritdoc />
    public void RemoveChild(ITransitionChild child)
    {
        if (child == null) throw new ArgumentNullException(nameof(child));

        _children.Remove(child);
    }

    /// <inheritdoc />
    public Task OnAfterRenderAsync()
    {
        if (_didRenderOnce) return Task.CompletedTask;

        _didRenderOnce = true;

        if (!Appear) return Task.CompletedTask;
        
        if (_showInitially) EnterTransition();
        else LeaveTransition();

        return Task.CompletedTask;
    }
}
