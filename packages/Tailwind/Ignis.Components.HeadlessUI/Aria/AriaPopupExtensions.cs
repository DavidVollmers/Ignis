using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Ignis.Components.HeadlessUI.Aria;

internal static class AriaPopupExtensions
{
    public static void SetActiveDescendant(this IAriaPopup popup, IAriaDescendant descendant, bool isActive)
    {
        if (descendant == null) throw new ArgumentNullException(nameof(descendant));

        if (isActive)
        {
            popup.ActiveDescendant = descendant;
            _ = ScrollIntoViewAsync(popup);
        }
        else if (popup.ActiveDescendant == descendant)
        {
            popup.ActiveDescendant = null;
        }
    }

    public static void OnKeyDown(this IAriaPopup popup, KeyboardEventArgs eventArgs)
    {
        var descendants = popup.Descendants.ToArray();

        switch (eventArgs.Code)
        {
            case "Escape":
                popup.Close();
                break;
            case "Space" or "Enter":
                if (popup.IsOpen)
                {
                    if (popup.ActiveDescendant != null) popup.ActiveDescendant.Click();
                    else popup.Close();
                }
                else
                {
                    popup.Open();
                }

                break;
            case "ArrowUp" when popup.ActiveDescendant == null:
            case "ArrowDown" when popup.ActiveDescendant == null:
                if (descendants.Any()) popup.SetActiveDescendant(descendants[0], isActive: true);
                else if (!popup.IsOpen) popup.Open();
                break;
            case "ArrowDown":
                {
                    var index = Array.IndexOf(descendants, popup.ActiveDescendant) + 1;
                    if (index < descendants.Length) popup.SetActiveDescendant(descendants[index], isActive: true);
                    else if (!popup.IsOpen) popup.Open();
                    break;
                }
            case "ArrowUp":
                {
                    var index = Array.IndexOf(descendants, popup.ActiveDescendant) - 1;
                    if (index >= 0) popup.SetActiveDescendant(descendants[index], isActive: true);
                    else if (!popup.IsOpen) popup.Open();
                    break;
                }
        }
    }

    private static readonly object ScrollOptions = new { Behavior = "smooth", Block = "nearest" };

    private static async Task ScrollIntoViewAsync(IAriaPopup popup)
    {
        var id = popup.GetId(popup.ActiveDescendant);
        if (id == null) return;

        await using var element = await popup.JSRuntime.InvokeAsync<IJSObjectReference>("document.getElementById", id);
        await element.InvokeVoidAsync("scrollIntoView", ScrollOptions);
    }
}
