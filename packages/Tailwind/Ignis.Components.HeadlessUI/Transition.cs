using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Transition : IgnisComponentBase, IDynamicComponent, ITransition
{
    private TransitionState _state = TransitionState.Default;
    private Type? _asComponent;
    private string? _asElement;
    private bool _isShowing;

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
            if (value) EnterCore();
            else LeaveCore();
        }
    }

    [Parameter] public string? Enter { get; set; }

    [Parameter] public string? EnterFrom { get; set; }

    [Parameter] public string? EnterTo { get; set; }

    [Parameter] public string? Leave { get; set; }

    [Parameter] public string? LeaveFrom { get; set; }

    [Parameter] public string? LeaveTo { get; set; }

    [Parameter] public RenderFragment<ITransition>? ChildContent { get; set; }

    [CascadingParameter] public IListbox? Listbox { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    public string? CssClass
    {
        get
        {
            var originalClassString = AdditionalAttributes?.ContainsKey("class") == true
                ? AdditionalAttributes["class"]
                : null;
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

    public IReadOnlyDictionary<string, object?> Attributes
    {
        get
        {
            var attributes = new Dictionary<string, object?> { { "class", CssClass } };

            // ReSharper disable once InvertIf
            if (AdditionalAttributes != null)
            {
                foreach (var (key, value) in AdditionalAttributes)
                {
                    if (key == "class") continue;

                    attributes[key] = value;
                }
            }

            return attributes;
        }
    }

    public Transition()
    {
        AsElement = "div";
    }

    protected override void OnInitialized()
    {
        Listbox?.SetTransition(this);
    }

    public void Hide(Action onHidden)
    {
        LeaveCore();

        onHidden();
    }

    void ITransition.Show()
    {
        EnterCore();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddContentFor(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }

    private void EnterCore()
    {
        if (_state != TransitionState.CanEnter && _state != TransitionState.Default) return;

        UpdateState(TransitionState.Entering);

        var duration = ParseDuration(Enter);
        if (duration != null) Task.Delay(duration.Value).GetAwaiter().GetResult();

        UpdateState(TransitionState.CanLeave);
    }

    private void LeaveCore()
    {
        if (_state != TransitionState.CanLeave && _state != TransitionState.Default) return;

        UpdateState(TransitionState.Leaving);

        var duration = ParseDuration(Leave);
        if (duration != null) Task.Delay(duration.Value).GetAwaiter().GetResult();
        
        UpdateState(TransitionState.CanEnter);
    }

    private void UpdateState(TransitionState state)
    {
        _state = state;
        
        _isShowing = state is TransitionState.Entering or TransitionState.CanLeave;
        
        ForceUpdate();
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
