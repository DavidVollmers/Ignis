using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Ignis.Components.Web;

public abstract class FocusComponentBase : IgnisComponentBase, IFocus, IHandleAfterRender, IDisposable
{
    private bool _isFocused;

    protected abstract IEnumerable<ElementReference> TargetElements { get; }

    // ReSharper disable once InconsistentNaming
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    protected virtual void OnFocus()
    {
    }

    protected virtual Task OnFocusAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual void OnBlur()
    {
    }

    protected virtual Task OnBlurAsync()
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// For internal use only.
    /// </summary>
    [JSInvokable]
    public async Task InvokeFocusAsync()
    {
        if (_isFocused) return;

        _isFocused = true;

        // ReSharper disable once MethodHasAsyncOverload
        OnFocus();

        await OnFocusAsync();
    }

    /// <summary>
    /// For internal use only.
    /// </summary>
    [JSInvokable]
    public async Task InvokeBlurAsync()
    {
        if (!_isFocused) return;

        _isFocused = false;

        // ReSharper disable once MethodHasAsyncOverload
        OnBlur();

        await OnBlurAsync();
    }

    public async Task FocusAsync()
    {
        if (!TargetElements.Any()) throw new InvalidOperationException("No element to focus.");

        await JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusComponentBase.focus", TargetElements);
    }

    /// <inheritdoc />
    public virtual async Task OnAfterRenderAsync()
    {
        if (!TargetElements.Any()) return;

        await JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusComponentBase.onAfterRender", TargetElements,
            _isFocused, DotNetObjectReference.Create(this));
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

#pragma warning disable CA2012
        JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusComponentBase.dispose", TargetElements);
#pragma warning restore CA2012
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~FocusComponentBase()
    {
        Dispose(false);
    }
}
