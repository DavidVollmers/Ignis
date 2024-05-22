using System.ComponentModel;
using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public abstract class OpenCloseWithTransitionComponentBase : FocusComponentBase, IOpenClose, IWithTransition
{
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

    [Inject] private IFrameTracker FrameTracker { get; set; } = null!;

    /// <inheritdoc />
    public Transition? Transition { get; set; }

    /// <inheritdoc />
    public void Open(Action? continueWith = null)
    {
        if (_isOpen || FrameTracker.IsPending)
        {
            if (_isOpen) continueWith?.Invoke();
            return;
        }

        _ = IsOpenChanged.InvokeAsync(_isOpen = true);

        if (Transition != null)
            FrameTracker.ExecuteOnNextFrame(this, () => Transition.EnterTransition(() => OnAfterOpen(continueWith)));
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

        if (Transition != null)
        {
            Transition.LeaveTransition(() => CloseCore(continueWith, async: true));
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
    public override async Task OnAfterRenderAsync()
    {
        FrameTracker.OnAfterRender();

        await base.OnAfterRenderAsync();
    }
}
