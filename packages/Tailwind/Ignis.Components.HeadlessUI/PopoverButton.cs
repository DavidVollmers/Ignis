using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class PopoverButton : DynamicComponentBase<PopoverButton>
{
    [Parameter]
    public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public Popover Popover { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public PopoverButton() : base("button")
    {
        //TODO aria-controls
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)),
            () => new KeyValuePair<string, object?>("aria-expanded", Popover.IsOpen.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("type",
                string.Equals(AsElement, "button", StringComparison.OrdinalIgnoreCase) ? "button" : null),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Popover == null)
        {
            throw new InvalidOperationException(
                $"{nameof(PopoverButton)} must be used inside a {nameof(HeadlessUI.Popover)}.");
        }

        Popover.SetButton(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor(3, this, ChildContent);
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }

    private void Click()
    {
        var @event = new ComponentEvent();

        var __ = OnClick.InvokeAsync(@event);

        if (@event.CancellationToken.IsCancellationRequested) return;

        if (Popover.IsOpen) Popover.Close();
        else Popover.Open();
    }
}
