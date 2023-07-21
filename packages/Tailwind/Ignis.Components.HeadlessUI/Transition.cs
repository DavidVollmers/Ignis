using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Transition : TransitionBase, ITransition
{
    private bool _isInitialized;
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
            _show = value;

            if (!_isInitialized) return;

            if (_show) EnterTransition();
            else LeaveTransition();
        }
    }

    /// <inheritdoc />
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

        _isInitialized = true;

        // ReSharper disable once InvertIf
        if (Appear)
        {
            if (_show) EnterTransition();
            else LeaveTransition();
        }
    }

    /// <inheritdoc />
    public void Hide(Action onHidden)
    {
        LeaveTransition(onHidden);
    }

    void ITransition.Show()
    {
        EnterTransition();
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<ITransition, Transition>(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }
}
