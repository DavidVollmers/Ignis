using System.Globalization;
using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public abstract class TransitionBase<T> : DynamicComponentBase<T>, ICssClass, IHandleAfterRender
    where T : TransitionBase<T>
{
    // this is needed for the transition to work properly and no frames are skipped.
    private const int TransitionGraceDuration = 10;

    private TransitionState _state = TransitionState.Default;

    protected bool RenderContent { get; private set; }

    [Parameter] public string? Enter { get; set; }

    [Parameter] public string? EnterFrom { get; set; }

    [Parameter] public string? EnterTo { get; set; }

    [Parameter] public string? Leave { get; set; }

    [Parameter] public string? LeaveFrom { get; set; }

    [Parameter] public string? LeaveTo { get; set; }

    /// <inheritdoc />
    public string? CssClass
    {
        get
        {
            var originalClassString = AdditionalAttributes?.FirstOrDefault(a =>
                string.Equals(a.Key, "class", StringComparison.OrdinalIgnoreCase));
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

    /// <inheritdoc cref="DynamicComponentBase{T}" />
    public new IEnumerable<KeyValuePair<string, object?>>? Attributes
    {
        get
        {
            if (base.Attributes != null)
            {
                foreach (var attribute in base.Attributes)
                {
                    if (string.Equals(attribute.Key, "class", StringComparison.OrdinalIgnoreCase)) continue;

                    yield return attribute;
                }
            }

            yield return new KeyValuePair<string, object?>("class", CssClass);
        }
    }

    [Inject] private IFrameTracker FrameTracker { get; set; } = null!;

    [Inject] internal TimeProvider TimeProvider { get; set; } = null!;

    protected TransitionBase(string asElement) : base(asElement)
    {
    }

    protected TransitionBase(Type asComponent) : base(asComponent)
    {
    }

    public virtual void EnterTransition(Action? continueWith = null)
    {
        if (_state != TransitionState.Default && _state != TransitionState.CanEnter) return;

        RenderContent = true;

        UpdateState(TransitionState.Entering, () =>
        {
            ITimer timer = null!;
            var (graceDuration, transitionDuration) = ParseDuration(Enter);
            timer = TimeProvider.CreateTimer(_ =>
            {
                // ReSharper disable once AccessToModifiedClosure
                timer.Dispose();
                UpdateState(TransitionState.Entered, () =>
                {
                    timer = TimeProvider.CreateTimer(_ =>
                    {
                        // ReSharper disable once AccessToModifiedClosure
                        timer.Dispose();
                        UpdateState(TransitionState.CanLeave, continueWith);
                    }, state: null, transitionDuration, Timeout.InfiniteTimeSpan);
                });
            }, state: null, graceDuration, Timeout.InfiniteTimeSpan);
        });
    }

    public virtual void LeaveTransition(Action? continueWith = null)
    {
        if (_state != TransitionState.Default && _state != TransitionState.CanLeave) return;

        RenderContent = true;

        UpdateState(TransitionState.Leaving, () =>
        {
            ITimer timer = null!;
            var (graceDuration, transitionDuration) = ParseDuration(Leave);
            timer = TimeProvider.CreateTimer(_ =>
            {
                // ReSharper disable once AccessToModifiedClosure
                timer.Dispose();
                UpdateState(TransitionState.Left, () =>
                {
                    timer = TimeProvider.CreateTimer(_ =>
                    {
                        // ReSharper disable once AccessToModifiedClosure
                        timer.Dispose();

                        RenderContent = false;

                        UpdateState(TransitionState.CanEnter, continueWith);
                    }, state: null, transitionDuration, Timeout.InfiniteTimeSpan);
                });
            }, state: null, graceDuration, Timeout.InfiniteTimeSpan);
        });
    }

    private void UpdateState(TransitionState state, Action? continueWith)
    {
        if (FrameTracker.IsPending) return;

        _state = state;

        if (continueWith != null) FrameTracker.ExecuteOnNextFrame(this, continueWith);

        Update(async: true);
    }

    /// <inheritdoc />
    public virtual Task OnAfterRenderAsync()
    {
        FrameTracker.OnAfterRender();

        return Task.CompletedTask;
    }

    private static (TimeSpan, TimeSpan) ParseDuration(string? classString)
    {
        var durationClass = classString?.Split(' ')
            .Select(v => v.Trim().Split(':')[v.Trim().Split(':').Length - 1])
            .FirstOrDefault(v => v.StartsWith("duration-", StringComparison.Ordinal));
        if (durationClass == null) return (TimeSpan.Zero, TimeSpan.Zero);

        var factor = 1;

        var durationString = durationClass.Split('-')[durationClass.Split('-').Length - 1];
        // ReSharper disable once InvertIf
        if (durationString.StartsWith('['))
        {
            durationString = durationString.TrimStart('[').TrimEnd(']').ToLowerInvariant();
            if (durationString.EndsWith("ms", StringComparison.Ordinal))
            {
                durationString = durationString[..^2];
            }
            else if (durationString.EndsWith('s'))
            {
                durationString = durationString[..^1];
                factor = 1000;
            }
            else return (TimeSpan.Zero, TimeSpan.Zero);
        }

        var duration = int.Parse(durationString, CultureInfo.InvariantCulture) * factor;
        if (duration <= 0) return (TimeSpan.Zero, TimeSpan.Zero);

        return duration < TransitionGraceDuration
            ? (TimeSpan.Zero, TimeSpan.FromMilliseconds(duration))
            : (TimeSpan.FromMilliseconds(TransitionGraceDuration),
                TimeSpan.FromMilliseconds(duration - TransitionGraceDuration));
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
