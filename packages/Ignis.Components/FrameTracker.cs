namespace Ignis.Components;

internal class FrameTracker
{
    private readonly IHostContext _hostContext;

    private long _currentFrame;
    private long? _frameToExecuteOn;
    private IgnisComponentBase? _target;
    private Action? _action;

    public bool IsPending => _action != null;

    public FrameTracker(IHostContext hostContext)
    {
        _hostContext = hostContext ?? throw new ArgumentNullException(nameof(hostContext));
    }

    public void ExecuteOnNextFrame(IgnisComponentBase target, Action action)
    {
        _target = target ?? throw new ArgumentNullException(nameof(target));
        _action = action ?? throw new ArgumentNullException(nameof(action));

        // If we're server-side, we can just execute the action on the next render, otherwise we need to wait for the second render. (WebAssembly)
        _frameToExecuteOn = _currentFrame + (_hostContext.IsServerSide ? 1 : 2);
    }

    public void OnAfterRender()
    {
        if (_currentFrame >= _frameToExecuteOn)
        {
            _frameToExecuteOn = null;

            _action?.Invoke();

            _action = null;
        }
        else if (_frameToExecuteOn.HasValue)
        {
            _target?.Update();
        }

        ++_currentFrame;
    }
}
