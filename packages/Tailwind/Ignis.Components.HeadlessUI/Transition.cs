using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Transition : TransitionBase, IDynamicParentComponent<Transition>, IContentProvider, IDisposable
{
    private readonly IList<TransitionChild> _children = new List<TransitionChild>();
    private readonly IList<Dialog> _dialogs = new List<Dialog>();

    private bool _transitioningTo;
    private bool _didRenderOnce;
    private bool _showInitially;
    private Type? _asComponent;
    private string? _asElement;
    private bool _isHosted;
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

            Trigger();
        }
    }

    [Parameter] public bool Appear { get; set; }

    [CascadingParameter] public IContentHost? Outlet { get; set; }

    [CascadingParameter] public Menu? Menu { get; set; }

    [CascadingParameter] public Listbox<object>? Listbox { get; set; }

    [CascadingParameter] public Popover? Popover { get; set; }

    [CascadingParameter] public Disclosure? Disclosure { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<Transition>? _ { get; set; }

    [Parameter] public RenderFragment<Transition>? ChildContent { get; set; }

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
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
        if (Outlet != null && _isHosted) return;

        BuildContentRenderTree(builder);
    }

    private void BuildContentRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<Transition>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<Transition>.IsFixed), value: true);
        builder.AddAttribute(2, nameof(CascadingValue<Transition>.Value), this);
        if (RenderContent || _show)
        {
            // ReSharper disable once VariableHidesOuterVariable
            builder.AddAttribute(3, nameof(CascadingValue<Transition>.ChildContent), (RenderFragment)(builder =>
            {
                builder.OpenAs(4, this);
                builder.AddMultipleAttributes(5, Attributes!);
                builder.AddChildContentFor(6, this, ChildContent);

                builder.CloseAs(this);
            }));
        }

        builder.CloseComponent();
    }

    /// <inheritdoc />
    public void HostedBy(IContentHost host)
    {
        Outlet = host ?? throw new ArgumentNullException(nameof(host));

        _isHosted = true;

        base.Update();
    }

    /// <inheritdoc />
    protected internal override void Update(bool async = false)
    {
        if (_isHosted) Outlet?.Update(async);

        base.Update(async);
    }

    public void Trigger(Action? continueWith = null)
    {
        if (_show) EnterTransition(continueWith);
        else LeaveTransition(continueWith);
    }

    /// <inheritdoc />
    protected override void EnterTransition(Action? continueWith = null)
    {
        _transitioningTo = _show = true;

        WatchTransition(isEnter: true, continueWith);
    }

    /// <inheritdoc />
    protected override void LeaveTransition(Action? continueWith = null)
    {
        _transitioningTo = false;

        WatchTransition(isEnter: false, () =>
        {
            _show = false;

            continueWith?.Invoke();
        });
    }

    public void AddChild(TransitionChild child)
    {
        if (child == null) throw new ArgumentNullException(nameof(child));

        if (!_children.Contains(child)) _children.Add(child);
    }

    public void RemoveChild(TransitionChild child)
    {
        if (child == null) throw new ArgumentNullException(nameof(child));

        _children.Remove(child);
    }

    public void AddDialog(Dialog dialog)
    {
        if (dialog == null) throw new ArgumentNullException(nameof(dialog));

        if (!_dialogs.Contains(dialog)) _dialogs.Add(dialog);

        if (Outlet == null) ContentRegistry.RegisterContentProvider(this);
    }

    public void RemoveDialog(Dialog dialog)
    {
        if (dialog == null) throw new ArgumentNullException(nameof(dialog));

        _dialogs.Remove(dialog);
    }

    private void WatchTransition(bool isEnter, Action? continueWith)
    {
        var startedTransitions = new List<TransitionChild>();
        var finishedTransitions = 0;

        if (isEnter) base.EnterTransition(() => AggregateDialogs(open: true, ContinueWith));
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
                else AggregateDialogs(open: false, () => base.LeaveTransition(continueWith));
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
