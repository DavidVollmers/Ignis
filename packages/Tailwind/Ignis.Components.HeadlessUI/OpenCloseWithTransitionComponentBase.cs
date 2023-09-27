using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public abstract class OpenCloseWithTransitionComponentBase : FocusComponentBase, IOpenClose, IWithTransition
{
    private ITransition? _transition;
    private bool _isOpen;

    /// <inheritdoc />
    [Parameter]
    public bool IsOpen
    {
        get => _isOpen;
        set
        {
            if (value) Open();
            else Close();
        }
    }

    /// <inheritdoc />
    [Parameter]
    public EventCallback<bool> IsOpenChanged { get; set; }

    [Inject] internal FrameTracker FrameTracker { get; set; } = null!;

    /// <inheritdoc />
    public void Open(Action? continueWith = null)
    {
        if (_isOpen || FrameTracker.IsPending)
        {
            if (_isOpen) continueWith?.Invoke();
            return;
        }

        _ = IsOpenChanged.InvokeAsync(_isOpen = true);

        if (_transition != null)
            FrameTracker.ExecuteOnNextFrame(this, () => _transition.Show(() => OnAfterOpen(continueWith)));
        else if (continueWith != null) FrameTracker.ExecuteOnNextFrame(this, () => OnAfterOpen(continueWith));

        Update();
    }

    protected virtual void OnAfterOpen(Action? continueWith)
    {
        _ = UpdateTargetsAsync();

        continueWith?.Invoke();
    }

    /// <inheritdoc />
    public void Close(Action? continueWith = null)
    {
        if (!_isOpen || FrameTracker.IsPending)
        {
            if (!_isOpen) continueWith?.Invoke();
            return;
        }

        if (_transition != null)
        {
            _transition.Hide(() => CloseCore(continueWith, true));
            return;
        }

        CloseCore(continueWith);
    }

    private void CloseCore(Action? continueWith, bool async = false)
    {
        _ = IsOpenChanged.InvokeAsync(_isOpen = false);

        if (continueWith != null) FrameTracker.ExecuteOnNextFrame(this, continueWith);

        Update(async);
    }

    /// <inheritdoc />
    public void SetTransition(ITransition transition)
    {
        _transition = transition ?? throw new ArgumentNullException(nameof(transition));
    }

    /// <inheritdoc />
    public override async Task OnAfterRenderAsync()
    {
        FrameTracker.OnAfterRender();

        await base.OnAfterRenderAsync();
    }
}
