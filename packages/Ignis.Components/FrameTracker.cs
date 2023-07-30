namespace Ignis.Components;

internal class FrameTracker
{
    private readonly IHostContext _hostContext;

    private Action? _action;

    public bool IsPending => _action != null;

    public FrameTracker(IHostContext hostContext)
    {
        _hostContext = hostContext ?? throw new ArgumentNullException(nameof(hostContext));
    }

    public void ExecuteOnNextFrame(Action action, Action<bool> update)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        // If we're server-side, we can just execute the action on the next render, otherwise we need to wait for the second render. (WebAssembly)
        _action = _hostContext.IsServerSide
            ? action
            : () =>
            {
                _action = action;

                update(false);
            };
    }

    public void OnAfterRender()
    {
        var action = _action;

        _action = null;
        
        action?.Invoke();
    }
}
