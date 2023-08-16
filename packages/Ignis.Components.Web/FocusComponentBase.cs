using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Ignis.Components.Web;

public abstract class FocusComponentBase : IgnisComponentBase, IFocus, IHandleAfterRender, IDisposable
{
    private readonly DotNetObjectReference<FocusComponentBase> _reference;

    private bool _isFocused;

    protected abstract IEnumerable<object> Targets { get; }

    protected virtual bool FocusOnRender => false;

    // ReSharper disable once InconsistentNaming
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    protected FocusComponentBase()
    {
        _reference = DotNetObjectReference.Create(this);
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
        if (!Targets.Any()) throw new InvalidOperationException("No element to focus.");

        await JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusComponentBase.focus", _reference);
    }

    /// <inheritdoc />
    public virtual async Task OnAfterRenderAsync()
    {
        if (!Targets.Any()) return;

        await JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusComponentBase.onAfterRender", _reference,
            GetElementReferences(), _isFocused, FocusOnRender);
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
                case IElementReferenceProvider elementReferenceProvider:
                    if (elementReferenceProvider.Element.HasValue) yield return elementReferenceProvider.Element.Value;
                    break;
                default:
                    throw new InvalidOperationException(
                        $"The type {target.GetType()} is not supported as a target. Only {nameof(ElementReference)} and {nameof(IElementReferenceProvider)} are supported.");
            }
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

#pragma warning disable CA2012
        JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusComponentBase.dispose", _reference);
#pragma warning restore CA2012

        _reference.Dispose();
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
