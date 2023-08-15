using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Ignis.Components.Web;

public abstract class FocusComponentBase : IgnisComponentBase, IFocus
{
    protected abstract ElementReference? TargetElement { get; }

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
    
    public async Task FocusAsync()
    {
        if (!TargetElement.HasValue) throw new InvalidOperationException("No element to focus.");
        
        await JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusComponentBase.focus", TargetElement);
    }
}
