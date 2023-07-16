using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Ignis.Components.Web;

public sealed class FocusDetector : IgnisComponentBase
{
    private ElementReference? _element;
    private bool? _isFocused;

    [Parameter] public EventCallback OnFocus { get; set; }

    [Parameter] public EventCallback OnBlur { get; set; }
    
    [Parameter] public string AsElement { get; set; } = "div";

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter, EditorRequired] public string Id { get; set; } = null!;

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? Attributes { get; set; }

    // ReSharper disable once InconsistentNaming
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    [JSInvokable]
    public async Task OnFocusAsync()
    {
        if (_isFocused.HasValue && _isFocused.Value) return;

        _isFocused = true;

        await OnFocus.InvokeAsync();
    }

    [JSInvokable]
    public async Task OnBlurAsync()
    {
        if (_isFocused.HasValue && !_isFocused.Value) return;

        _isFocused = false;

        await OnBlur.InvokeAsync();
    }

    protected override void OnInitialized()
    {
        var _ = JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusDetector", Id, _element, DotNetObjectReference.Create(this));
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, AsElement);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddAttribute(2, "id", Id);
        builder.AddElementReferenceCapture(3, element => _element = element);

        builder.AddContent(4, ChildContent);

        builder.CloseElement();
    }
}
