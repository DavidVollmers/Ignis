using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Transition : IgnisComponentBase, ITransition
{
    private TransitionState _state = TransitionState.Default;
    private bool _isInitialized;
    private Type? _asComponent;
    private string? _asElement;
    private bool _isShowing;

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
        get => _isShowing;
        set
        {
            if (!_isInitialized)
            {
                _isShowing = value;
                return;
            }

            if (value) EnterCore();
            else LeaveCore();
        }
    }

    [Parameter] public bool Appear { get; set; }

    [Parameter] public string? Enter { get; set; }

    [Parameter] public string? EnterFrom { get; set; }

    [Parameter] public string? EnterTo { get; set; }

    [Parameter] public string? Leave { get; set; }

    [Parameter] public string? LeaveFrom { get; set; }

    [Parameter] public string? LeaveTo { get; set; }

    [CascadingParameter] public IListbox? Listbox { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<ITransition>? _ { get; set; }

    [Parameter] public RenderFragment<ITransition>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public string? CssClass
    {
        get
        {
            var originalClassString = AdditionalAttributes?.FirstOrDefault(a => a.Key == "class");
            return _state switch
            {
                TransitionState.CanEnter => $"{originalClassString} {Enter} {EnterFrom}".Trim(),
                TransitionState.Entering => $"{originalClassString} {Enter} {EnterTo}".Trim(),
                TransitionState.CanLeave => $"{originalClassString} {Leave} {LeaveFrom}".Trim(),
                TransitionState.Leaving => $"{originalClassString} {Leave} {LeaveTo}".Trim(),
                _ => null
            };
        }
    }

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes
    {
        get
        {
            if (AdditionalAttributes != null)
            {
                foreach (var attribute in AdditionalAttributes)
                {
                    if (attribute.Key == "class") continue;

                    yield return attribute;
                }
            }

            yield return new KeyValuePair<string, object?>("class", CssClass);
        }
    }

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
            if (_isShowing) EnterCore();
            else LeaveCore();
        }
    }

    /// <inheritdoc />
    public void Hide(Action onHidden)
    {
        LeaveCore(onHidden);
    }

    void ITransition.Show()
    {
        EnterCore();
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<ITransition, Transition>(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }

    private void EnterCore()
    {
        if (_state != TransitionState.CanEnter && _state != TransitionState.Default) return;

        UpdateState(TransitionState.Entering);

        var duration = ParseDuration(Enter);
        if (duration != null)
        {
            Task.Delay(duration.Value).ContinueWith(_ => UpdateState(TransitionState.CanLeave, true));
            return;
        }

        UpdateState(TransitionState.CanLeave);
    }

    private void LeaveCore(Action? continueWith = null)
    {
        if (_state != TransitionState.CanLeave && _state != TransitionState.Default) return;

        UpdateState(TransitionState.Leaving);

        var duration = ParseDuration(Leave);
        if (duration != null)
        {
            Task.Delay(duration.Value).ContinueWith(_ =>
            {
                UpdateState(TransitionState.CanEnter, true);

                continueWith?.Invoke();
            });
            return;
        }

        UpdateState(TransitionState.CanEnter);

        continueWith?.Invoke();
    }

    private void UpdateState(TransitionState state, bool async = false)
    {
        _state = state;

        _isShowing = state is TransitionState.Entering or TransitionState.CanLeave;

        ForceUpdate(async);
    }

    private static int? ParseDuration(string? classString)
    {
        var durationClass = classString?.Split(' ')
            .Select(v => v.Trim().Split(':').Last())
            .FirstOrDefault(v => v.StartsWith("duration-"));
        if (durationClass == null) return null;

        var factor = 1;

        var durationString = durationClass.Split('-').Last();
        // ReSharper disable once InvertIf
        if (durationString.StartsWith('['))
        {
            durationString = durationString.TrimStart('[').TrimEnd(']').ToLowerInvariant();
            if (durationString.EndsWith("ms"))
            {
                durationString = durationString[..^2];
            }
            else if (durationString.EndsWith("s"))
            {
                durationString = durationString[..^1];
                factor = 1000;
            }
        }

        return int.Parse(durationString) * factor;
    }

    private enum TransitionState
    {
        Default,
        CanEnter,
        Entering,
        CanLeave,
        Leaving
    }
}
