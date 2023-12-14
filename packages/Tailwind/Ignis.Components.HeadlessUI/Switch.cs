using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Switch : DynamicComponentBase<Switch>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [Parameter] public bool Checked { get; set; }

    [Parameter] public EventCallback<bool> CheckedChanged { get; set; }

    [Parameter] public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public SwitchGroup? SwitchGroup { get; set; }

    [Parameter] public RenderFragment<Switch>? ChildContent { get; set; }

    public Switch() : base("button")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", SwitchGroup?.GetId(this) ?? Id),
            () => new KeyValuePair<string, object?>("tabindex", "0"),
            () => new KeyValuePair<string, object?>("role", "switch"),
            () => new KeyValuePair<string, object?>("aria-checked", Checked.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)), () =>
                new KeyValuePair<string, object?>("type",
                    string.Equals(AsElement, "button", StringComparison.OrdinalIgnoreCase) ? "button" : null),
            () =>
                new KeyValuePair<string, object?>("aria-labelledby", SwitchGroup?.GetId(SwitchGroup?.Label)),
            () => new KeyValuePair<string, object?>("aria-describedby", SwitchGroup?.GetId(SwitchGroup?.Description)),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (SwitchGroup != null) SwitchGroup.Switch = this;
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }

    public void Toggle()
    {
        var __ = CheckedChanged.InvokeAsync(Checked = !Checked);

        Update();
    }

    private void Click()
    {
        var @event = new ComponentEvent();

        var __ = OnClick.InvokeAsync(@event);

        if (@event.DefaultPrevented) return;

        Toggle();
    }
}
