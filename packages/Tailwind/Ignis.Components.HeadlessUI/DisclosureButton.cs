using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class DisclosureButton : DynamicComponentBase<DisclosureButton>
{
    [Parameter]
    public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public Disclosure Disclosure { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public DisclosureButton() : base("button")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)),
            () => new KeyValuePair<string, object?>("aria-expanded", Disclosure.IsOpen.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("type",
                string.Equals(AsElement, "button", StringComparison.OrdinalIgnoreCase) ? "button" : null),
            () => new KeyValuePair<string, object?>("aria-controls", Disclosure.GetId(Disclosure.Controlled)),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Disclosure == null)
        {
            throw new InvalidOperationException(
                $"{nameof(DisclosureButton)} must be used inside a {nameof(HeadlessUI.Disclosure)}.");
        }

        Disclosure.Button = this;
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

        if (@event.DefaultPrevented) return;

        Toggle();
    }

    private void Toggle()
    {
        if (Disclosure.IsOpen) Disclosure.Close();
        else Disclosure.Open();
    }
}
