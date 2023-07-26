using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public abstract class TransitionBase : IgnisComponentBase, ICssClass, IHandleAfterRender
{
    private TransitionState _state = TransitionState.Default;

    protected bool RenderContent { get; private set; }

    [Parameter] public string? Enter { get; set; }

    [Parameter] public string? EnterFrom { get; set; }

    [Parameter] public string? EnterTo { get; set; }

    [Parameter] public string? Leave { get; set; }

    [Parameter] public string? LeaveFrom { get; set; }

    [Parameter] public string? LeaveTo { get; set; }

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
                TransitionState.Entering => $"{originalClassString?.Value} {Enter} {EnterFrom}".Trim(),
                TransitionState.Entered => $"{originalClassString?.Value} {Enter} {EnterTo}".Trim(),
                TransitionState.CanLeave => $"{originalClassString?.Value} {EnterTo}".Trim(),
                TransitionState.Leaving => $"{originalClassString?.Value} {Leave} {LeaveFrom}".Trim(),
                TransitionState.Left => $"{originalClassString?.Value} {Leave} {LeaveTo}".Trim(),
                TransitionState.CanEnter => $"{originalClassString?.Value} {LeaveTo}".Trim(),
                _ => originalClassString?.Value?.ToString()
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

    [Inject] internal FrameTracker FrameTracker { get; set; }

    protected virtual void EnterTransition(Action? continueWith = null)
    {
        if (_state != TransitionState.Default && _state != TransitionState.CanEnter) return;

        RenderContent = true;

        UpdateState(TransitionState.Entering, () =>
        {
            UpdateState(TransitionState.Entered, () =>
            {
                var duration = ParseDuration(Enter);
                Task.Delay(duration ?? 0).ContinueWith(_ =>
                {
                    UpdateState(TransitionState.CanLeave, continueWith);
                });
            });
        });
    }

    protected virtual void LeaveTransition(Action? continueWith = null)
    {
        if (_state != TransitionState.Default && _state != TransitionState.CanLeave) return;

        RenderContent = true;

        UpdateState(TransitionState.Leaving, () =>
        {
            UpdateState(TransitionState.Left, () =>
            {
                var duration = ParseDuration(Leave);
                Task.Delay(duration ?? 0).ContinueWith(_ =>
                {
                    RenderContent = false;

                    UpdateState(TransitionState.CanEnter, continueWith);
                });
            });
        });
    }

    private void UpdateState(TransitionState state, Action? continueWith)
    {
        if (FrameTracker.IsPending) return;

        _state = state;

        if (continueWith != null) FrameTracker.ExecuteOnNextFrame(continueWith);

        ForceUpdate(true);
    }

    /// <inheritdoc />
    public virtual Task OnAfterRenderAsync()
    {
        FrameTracker.OnAfterRender();

        return Task.CompletedTask;
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
            else return null;
        }

        return int.Parse(durationString) * factor;
    }

    private enum TransitionState
    {
        Default,
        CanEnter,
        Entering,
        Entered,
        CanLeave,
        Leaving,
        Left
    }
}
