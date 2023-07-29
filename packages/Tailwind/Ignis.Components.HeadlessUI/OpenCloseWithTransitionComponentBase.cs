﻿using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public abstract class OpenCloseWithTransitionComponentBase : IgnisComponentBase, IOpenClose, IWithTransition,
    IHandleAfterRender
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
            FrameTracker.ExecuteOnNextFrame(() => _transition.Show(() => OnAfterOpen(continueWith)), ForceUpdate);
        else if (continueWith != null) FrameTracker.ExecuteOnNextFrame(() => OnAfterOpen(continueWith), ForceUpdate);

        ForceUpdate();
    }

    protected virtual void OnAfterOpen(Action? continueWith)
    {
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

        if (continueWith != null) FrameTracker.ExecuteOnNextFrame(continueWith, ForceUpdate);

        ForceUpdate(async);
    }

    /// <inheritdoc />
    public void SetTransition(ITransition transition)
    {
        _transition = transition ?? throw new ArgumentNullException(nameof(transition));
    }

    /// <inheritdoc />
    public Task OnAfterRenderAsync()
    {
        FrameTracker.OnAfterRender();

        return Task.CompletedTask;
    }
}