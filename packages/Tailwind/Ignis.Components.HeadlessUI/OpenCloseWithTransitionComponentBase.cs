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
        if (_isOpen || FrameTracker.IsPending) return;

        IsOpenChanged.InvokeAsync(_isOpen = true);

        if (_transition != null)
            FrameTracker.ExecuteOnNextFrame(() => _transition.Show(() => OnAfterOpen(continueWith)), Update);
        else if (continueWith != null) FrameTracker.ExecuteOnNextFrame(() => OnAfterOpen(continueWith), Update);

        Update();
    }

    protected virtual void OnAfterOpen(Action? continueWith)
    {
#pragma warning disable CS4014
        UpdateTargetsAsync();
#pragma warning restore CS4014
        
        continueWith?.Invoke();
    }

    /// <inheritdoc />
    public void Close(Action? continueWith = null)
    {
        if (!_isOpen || FrameTracker.IsPending) return;

        if (_transition != null)
        {
            _transition.Hide(() => CloseCore(continueWith, true));
            return;
        }

        CloseCore(continueWith);
    }

    private void CloseCore(Action? continueWith, bool async = false)
    {
        IsOpenChanged.InvokeAsync(_isOpen = false);

        if (continueWith != null) FrameTracker.ExecuteOnNextFrame(continueWith, Update);

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
