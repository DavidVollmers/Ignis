using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Ignis.Components.Web;

public abstract class FocusComponentBase : IgnisComponentBase, IFocus, IHandleAfterRender, IDisposable
{
    private readonly DotNetObjectReference<FocusComponentBase> _reference;

    private bool _isFocused;

    protected abstract IEnumerable<object> Targets { get; }

    protected virtual IEnumerable<string> KeysToCapture => Enumerable.Empty<string>();

    protected virtual bool FocusOnRender => false;

    // ReSharper disable once InconsistentNaming
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    protected FocusComponentBase()
    {
        _reference = DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// For internal use only.
    /// </summary>
    [JSInvokable]
    public async Task InvokeFocusAsync()
    {
        if (_isFocused) return;

        _isFocused = true;

#pragma warning disable MA0042
        // ReSharper disable once MethodHasAsyncOverload
        OnFocus();
#pragma warning restore MA0042

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

#pragma warning disable MA0042
        // ReSharper disable once MethodHasAsyncOverload
        OnBlur();
#pragma warning restore MA0042

        await OnBlurAsync();
    }

    /// <summary>
    /// For internal use only.
    /// </summary>
    [JSInvokable]
    public async Task InvokeKeyDownAsync(KeyboardEventArgs args)
    {
        if (!_isFocused || !KeysToCapture.Contains(args.Code, StringComparer.Ordinal)) return;

#pragma warning disable MA0042
        // ReSharper disable once MethodHasAsyncOverload
        OnKeyDown(args);
#pragma warning restore MA0042

        await OnKeyDownAsync(args);
    }

    public async Task FocusAsync()
    {
        if (!Targets.Any()) throw new InvalidOperationException("No element to focus.");

        await JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusComponentBase.focus", _reference);
    }

    /// <inheritdoc />
    public virtual async Task OnAfterRenderAsync()
    {
        if (!Targets.Any()) return;

        await UpdateTargetsAsync();
    }

    protected async Task UpdateTargetsAsync()
    {
        await JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusComponentBase.updateTargets", _reference,
            GetElementReferences(), _isFocused, FocusOnRender, KeysToCapture);
    }

    private IEnumerable<ElementReference> GetElementReferences()
    {
        foreach (var target in Targets)
        {
            switch (target)
            {
                case ElementReference elementReference:
                    yield return elementReference;
                    break;
                case IDynamicComponent dynamicComponent:
                    var reference = dynamicComponent.TryProvideElementReference();
                    if (reference.HasValue) yield return reference.Value;
                    break;
                default:
                    throw new InvalidOperationException(
                        $"The type {target.GetType()} is not supported as a target. Only {nameof(ElementReference)} and {nameof(IElementReferenceProvider)} are supported.");
            }
        }
    }

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

    protected virtual void OnKeyDown(KeyboardEventArgs args)
    {
    }

    protected virtual Task OnKeyDownAsync(KeyboardEventArgs args)
    {
        return Task.CompletedTask;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

#pragma warning disable CA2012
        _ = JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusComponentBase.dispose", _reference);
#pragma warning restore CA2012

        _reference.Dispose();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
