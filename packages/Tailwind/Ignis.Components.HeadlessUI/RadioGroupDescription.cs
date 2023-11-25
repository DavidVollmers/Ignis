using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroupDescription : DynamicComponentBase<RadioGroupDescription>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter] public string? Id { get; set; }

    [CascadingParameter(Name = nameof(RadioGroup<object>))]
    public IAriaLabeled RadioGroup { get; set; } = null!;

    [CascadingParameter(Name = nameof(RadioGroupOption<object>))]
    public IAriaDescribed? RadioGroupOption { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public RadioGroupDescription() : base("div")
    {
        SetAttributes(new[] { () => new KeyValuePair<string, object?>("id", Id ?? RadioGroup.Id + "-description"), });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (RadioGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(RadioGroupDescription)} must be used inside a {nameof(RadioGroup<object>)}.");
        }

        if (RadioGroupOption != null) RadioGroupOption.Description = this;
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
}
