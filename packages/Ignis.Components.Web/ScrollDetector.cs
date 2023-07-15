using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Ignis.Components.Web;

public sealed class ScrollDetector : IgnisComponentBase
{
    public EventCallback OnScroll { get; set; }

    // ReSharper disable once InconsistentNaming
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    [JSInvokable]
    public Task OnScrollAsync()
    {
        return OnScroll.InvokeAsync();
    }
    
    protected override void OnInitialized()
    {
        var _ = JSRuntime.InvokeVoidAsync("Ignis.Components.Web.ScrollDetector", DotNetObjectReference.Create(this));
    }
}
