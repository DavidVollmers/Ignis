using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroupLabel : DynamicComponentBase<RadioGroupLabel>
{
    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter] public RadioGroup<object> RadioGroup { get; set; } = null!;

    [CascadingParameter] public RadioGroupOption<object>? RadioGroupOption { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public RadioGroupLabel() : base("label")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id ?? RadioGroup.Id + "-label"), () =>
                new KeyValuePair<string, object?>("onclick",
                    RadioGroupOption != null ? EventCallback.Factory.Create(this, RadioGroupOption.FocusAsync) : null)
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (RadioGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(RadioGroupLabel)} must be used inside a {nameof(RadioGroup<object>)}.");
        }

        if (RadioGroupOption != null)
        {
            RadioGroupOption.SetLabel(this);
        }
        else
        {
            RadioGroup.SetLabel(this);
        }
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
