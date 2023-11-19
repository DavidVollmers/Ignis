using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class TabPanel : DynamicComponentBase<TabPanel>, IDisposable
{
    [CascadingParameter] public TabGroup TabGroup { get; set; } = null!;

    [Parameter] public RenderFragment<TabPanel>? ChildContent { get; set; }

    /// <inheritdoc />
    public bool IsSelected => TabGroup.IsTabPanelSelected(this);

    public TabPanel() : base("div")
    {
        //TODO aria-labelledby
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("role", "tabpanel"),
            () => new KeyValuePair<string, object?>("tabindex", IsSelected ? 0 : -1),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (TabGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(TabPanel)} must be used inside a {nameof(HeadlessUI.TabGroup)}.");
        }

        TabGroup.AddTabPanel(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (!IsSelected) return;

        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }

    public void Dispose()
    {
        TabGroup.RemoveTabPanel(this);
    }
}
