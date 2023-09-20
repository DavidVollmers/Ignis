using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Transition : TransitionBase, ITransition, IDisposable
{
    private readonly IList<ITransitionChild> _children = new List<ITransitionChild>();
    private readonly IList<IDialog> _dialogs = new List<IDialog>();

    private bool _transitioningTo;
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

            if (_transitioningTo == _show) return;

            if (_show) EnterTransition();
            else LeaveTransition();
        }
    }

    [Parameter] public bool Appear { get; set; }

    /// <inheritdoc />
    [CascadingParameter] public IContentHost? Outlet { get; set; }

    [CascadingParameter] public IMenu? Menu { get; set; }

    [CascadingParameter] public IListbox? Listbox { get; set; }

    [CascadingParameter] public IPopover? Popover { get; set; }

    [CascadingParameter] public IDisclosure? Disclosure { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<ITransition>? _ { get; set; }

    [Parameter] public RenderFragment<ITransition>? ChildContent { get; set; }

    /// <inheritdoc cref="IDynamicParentComponent{T}.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public RenderFragment Content => BuildContentRenderTree;

    /// <inheritdoc />
    public bool HasOutletDialogs => _dialogs.Any(d => !d.IgnoreOutlet);

    [Inject] public IContentRegistry ContentRegistry { get; set; } = null!;

    public Transition()
    {
        AsElement = "div";
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        Menu?.SetTransition(this);

        Listbox?.SetTransition(this);

        Popover?.SetTransition(this);

        Disclosure?.SetTransition(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (Outlet != null) return;

        BuildContentRenderTree(builder);
    }

    private void BuildContentRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<CascadingValue<ITransition>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<ITransition>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<ITransition>.Value), this);
            if (RenderContent || _show)
                builder.AddAttribute(6, nameof(CascadingValue<ITransition>.ChildContent),
                    this.GetChildContent(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void HostedBy(IContentHost host)
    {
        Outlet = host ?? throw new ArgumentNullException(nameof(host));

        Update();
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
        _transitioningTo = _show = true;

        WatchTransition(true, continueWith);
    }

    /// <inheritdoc />
    protected override void LeaveTransition(Action? continueWith = null)
    {
        _transitioningTo = false;

        WatchTransition(false, () =>
        {
            _show = false;

            continueWith?.Invoke();
        });
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
    public void AddDialog(IDialog dialog)
    {
        if (dialog == null) throw new ArgumentNullException(nameof(dialog));

        if (!_dialogs.Contains(dialog)) _dialogs.Add(dialog);

        if (Outlet == null) ContentRegistry.RegisterContentProvider(this);
    }

    /// <inheritdoc />
    public void RemoveDialog(IDialog dialog)
    {
        if (dialog == null) throw new ArgumentNullException(nameof(dialog));

        _dialogs.Remove(dialog);
    }

    private void WatchTransition(bool isEnter, Action? continueWith)
    {
        var startedTransitions = new List<ITransitionChild>();
        var finishedTransitions = 0;

        if (isEnter) base.EnterTransition(() => AggregateDialogs(true, ContinueWith));
        else ContinueWith();
        return;

        void ContinueWith()
        {
            ++finishedTransitions;

            foreach (var child in _children)
            {
                if (startedTransitions.Contains(child)) continue;

                startedTransitions.Add(child);

                if (isEnter) child.Show(ContinueWith);
                else child.Hide(ContinueWith);
            }

            // ReSharper disable once InvertIf
            if (finishedTransitions == startedTransitions.Count + 1)
            {
                if (isEnter) continueWith?.Invoke();
                else AggregateDialogs(false, () => base.LeaveTransition(continueWith));
            }
        }
    }

    private void AggregateDialogs(bool open, Action continueWith)
    {
        if (_dialogs.Count == 0)
        {
            continueWith();
            return;
        }

        var count = _dialogs.Count;

        foreach (var dialog in _dialogs)
        {
            if (open) dialog.Open(ContinueWith);
            else dialog.CloseFromTransition(ContinueWith);
        }

        return;

        void ContinueWith()
        {
            --count;

            if (count == 0) continueWith();
        }
    }

    /// <inheritdoc />
    public override Task OnAfterRenderAsync()
    {
        if (_didRenderOnce) return base.OnAfterRenderAsync();

        _didRenderOnce = true;

        if (!Appear) return base.OnAfterRenderAsync();

        if (_showInitially) EnterTransition();
        else LeaveTransition();

        return base.OnAfterRenderAsync();
    }

    public void Dispose()
    {
        ContentRegistry.UnregisterContentProvider(this);
    }
}
