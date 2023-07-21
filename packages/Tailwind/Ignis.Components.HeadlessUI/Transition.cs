using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Transition : TransitionBase, ITransition
{
    private readonly IList<ITransitionChild> _children = new List<ITransitionChild>();

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
            ShowInitially = _show = value;

            if (!DidRenderOnce) return;

            if (_show) ((ITransition)this).Show();
            else Hide();
        }
    }

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
    public void Hide(Action? onHidden = null)
    {
        var aggregatedOnHidden = AggregatedOnHidden(onHidden);

        LeaveTransition(aggregatedOnHidden);

        foreach (var child in _children)
        {
            child.Hide(aggregatedOnHidden);
        }
    }

    void ITransition.Show()
    {
        EnterTransition();

        foreach (var child in _children)
        {
            child.Show();
        }
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

    private Action? AggregatedOnHidden(Action? callback = null)
    {
        if (callback == null) return null;

        var count = _children.Count + 1;

        return () =>
        {
            count--;

            if (count == 0) callback();
        };
    }
}
