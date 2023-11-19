using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class Switch : DynamicComponentBase<Switch>
{
    [Parameter]
    public string? Id { get; set; }

    [Parameter] public bool Checked { get; set; }

    [Parameter] public EventCallback<bool> CheckedChanged { get; set; }

    [Parameter]
    public EventCallback<IComponentEvent> OnClick { get; set; }

    [CascadingParameter] public SwitchGroup? SwitchGroup { get; set; }

    [Parameter] public RenderFragment<Switch>? ChildContent { get; set; }

    public Switch() : base("button")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id",
                Id != null || SwitchGroup != null ? Id ?? SwitchGroup?.Id + "-button" : null),
            () => new KeyValuePair<string, object?>("tabindex", "0"),
            () => new KeyValuePair<string, object?>("role", "switch"),
            () => new KeyValuePair<string, object?>("aria-checked", Checked.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Click)),
            () => new KeyValuePair<string, object?>("type",
                string.Equals(AsElement, "button", StringComparison.OrdinalIgnoreCase) ? "button" : null),
            () =>
                new KeyValuePair<string, object?>("aria-labelledby",
                    SwitchGroup?.Label == null ? null : SwitchGroup.Label.Id ?? SwitchGroup.Id + "-label"),
            () => new KeyValuePair<string, object?>("aria-describedby",
                SwitchGroup?.Description == null ? null : SwitchGroup.Description.Id ?? SwitchGroup.Id + "-description"),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        SwitchGroup?.SetSwitch(this);
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

        if (@event.CancellationToken.IsCancellationRequested) return;

        Toggle();
    }
}
