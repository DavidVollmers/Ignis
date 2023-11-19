using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class MenuItems : DynamicComponentBase<MenuItems>
{
    [CascadingParameter] public Menu Menu { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public MenuItems() : base("div")
    {
        //TODO aria-active-descendant
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("tabindex", -1),
            () => new KeyValuePair<string, object?>("role", "menu"), () => new KeyValuePair<string, object?>(
                "aria-labelledby", Menu.Button == null ? null : Menu.Button.Id ?? Menu.Id + "-button")
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Menu == null)
        {
            throw new InvalidOperationException(
                $"{nameof(MenuItems)} must be used inside a {nameof(HeadlessUI.Menu)}.");
        }

        Menu.SetItems(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!Menu.IsOpen) return;

        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        builder.AddChildContentFor(3, this, ChildContent);
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }
}
