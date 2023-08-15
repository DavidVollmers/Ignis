using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Ignis.Components.Web;

public sealed class ScrollDetector : IgnisAsyncComponentBase
{
    private readonly DotNetObjectReference<ScrollDetector> _reference;

    [Parameter] public EventCallback<ScrollEventArgs> OnScroll { get; set; }

    // ReSharper disable once InconsistentNaming
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    public ScrollDetector()
    {
        _reference = DotNetObjectReference.Create(this);
    }

    [JSInvokable]
    public async Task InvokeScrollAsync(int scrollX, int scrollY)
    {
        await OnScroll.InvokeAsync(new ScrollEventArgs(scrollX, scrollY));
    }

    protected override async Task OnInitializedAsync(CancellationToken cancellationToken)
    {
        await JSRuntime.InvokeVoidAsync("Ignis.Components.Web.ScrollDetector", cancellationToken, _reference);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing) _reference.Dispose();

        base.Dispose(disposing);
    }
}
