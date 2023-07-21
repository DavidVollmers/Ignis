using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Ignis.Components.Web;

public sealed class FocusDetector : IgnisComponentBase, IHandleAfterRender
{
    private ElementReference? _element;
    private bool? _isFocused;

    [Parameter] public EventCallback OnFocus { get; set; }

    [Parameter] public EventCallback OnBlur { get; set; }

    [Parameter] public string AsElement { get; set; } = "div";

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter, EditorRequired] public string Id { get; set; } = null!;

    [Parameter] public bool Strict { get; set; } = true;

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    // ReSharper disable once InconsistentNaming
    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    [JSInvokable]
    public async Task OnFocusAsync()
    {
        if (Strict && _isFocused.HasValue && _isFocused.Value) return;

        _isFocused = true;

        await OnFocus.InvokeAsync();
    }

    [JSInvokable]
    public async Task OnBlurAsync()
    {
        if (Strict && _isFocused.HasValue && !_isFocused.Value) return;

        _isFocused = false;

        await OnBlur.InvokeAsync();
    }

    public async Task OnAfterRenderAsync()
    {
        if (Server.IsPrerendering || _isFocused.HasValue) return;
        
        await JSRuntime.InvokeVoidAsync("Ignis.Components.Web.FocusDetector", DotNetObjectReference.Create(this), Id, _element);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, AsElement);
        builder.AddMultipleAttributes(1, AdditionalAttributes!);
        builder.AddAttribute(2, "id", Id);
        builder.AddElementReferenceCapture(3, element =>
        {
            _isFocused = null;
            _element = element;
        });
        builder.AddContent(4, ChildContent);

        builder.CloseElement();
    }
}
