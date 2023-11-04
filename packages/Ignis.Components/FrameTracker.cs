namespace Ignis.Components;

internal class FrameTracker : IFrameTracker
{
    private long _currentFrame;
    private long? _frameToExecuteOn;
    private IgnisComponentBase? _target;
    private Action? _action;

    public bool IsPending => _action != null;

    public void ExecuteOnNextFrame(IgnisComponentBase target, Action action)
    {
        _target = target ?? throw new ArgumentNullException(nameof(target));
        _action = action ?? throw new ArgumentNullException(nameof(action));

        _frameToExecuteOn = _currentFrame + 1;
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
