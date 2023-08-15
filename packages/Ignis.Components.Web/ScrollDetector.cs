using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Ignis.Components.Web;

public sealed class ScrollDetector : IgnisAsyncComponentBase
{
    [Parameter] public EventCallback<ScrollEventArgs> OnScroll { get; set; }

    // ReSharper disable once InconsistentNaming
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    [JSInvokable]
    public async Task OnScrollAsync(int scrollX, int scrollY)
    {
        await OnScroll.InvokeAsync(new ScrollEventArgs(scrollX, scrollY));
    }

    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        await JSRuntime.InvokeVoidAsync("Ignis.Components.Web.ScrollDetector", cancellationToken,
            DotNetObjectReference.Create(this));
    }
}
