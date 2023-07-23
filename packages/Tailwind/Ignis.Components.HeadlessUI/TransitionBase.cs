using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public abstract class TransitionBase : IgnisComponentBase, ICssClass, IHandleAfterRender
{
    private TransitionState _state = TransitionState.Default;

    protected bool IsShowing { get; private set; }
    
    protected bool DidRenderOnce { get; private set; }
    
    protected bool ShowInitially { get; set; }

    [Parameter] public string? Enter { get; set; }

    [Parameter] public string? EnterFrom { get; set; }

    [Parameter] public string? EnterTo { get; set; }

    [Parameter] public string? Leave { get; set; }

    [Parameter] public string? LeaveFrom { get; set; }

    [Parameter] public string? LeaveTo { get; set; }

    [Parameter] public bool Appear { get; set; }

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

    /// <inheritdoc cref="IDynamicComponent" />
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

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    protected void EnterTransition(Action? continueWith = null)
    {
        if (_state != TransitionState.CanEnter && _state != TransitionState.Default) return;

        IsShowing = true;
        
        UpdateState(TransitionState.Entering);

        var duration = ParseDuration(Enter);
        if (duration != null)
        {
            Task.Delay(duration.Value).ContinueWith(_ =>
            {
                UpdateState(TransitionState.CanLeave, true);

                continueWith?.Invoke();
            });
            return;
        }

        UpdateState(TransitionState.CanLeave);

        continueWith?.Invoke();
    }

    protected void LeaveTransition(Action? continueWith = null)
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
        
                IsShowing = true;
        
                ForceUpdate(true);
            });
            return;
        }

        UpdateState(TransitionState.CanEnter);

        continueWith?.Invoke();
        
        IsShowing = true;
        
        ForceUpdate();
    }

    private void UpdateState(TransitionState state, bool async = false)
    {
        _state = state;

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

    /// <inheritdoc />
    public async Task OnAfterRenderAsync()
    {
        if (DidRenderOnce) return;
        
        DidRenderOnce = true;

        // ReSharper disable once InvertIf
        if (Appear)
        {
            if (ShowInitially) EnterTransition();
            else LeaveTransition();
        }
    }
}
