namespace Ignis.Components;

internal class NextFrameTracker
{
    private readonly IHostContext _hostContext;

    private Action? _action;

    public NextFrameTracker(IHostContext hostContext)
    {
        _hostContext = hostContext ?? throw new ArgumentNullException(nameof(hostContext));
    }

    public void ExecuteOnNextFrame(Action action)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        _action = _hostContext.IsServerSide
            ? action
            : () =>
            {
                _action = action;
            };
    }

    public void OnAfterRender()
    {
        _action?.Invoke();
    }
}
