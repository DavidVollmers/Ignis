using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroupLabel : DynamicComponentBase<RadioGroupLabel>, IAriaComponentPart
{
    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter(Name = nameof(RadioGroup<object>))]
    public IAriaCheckGroup RadioGroup { get; set; } = null!;

    [CascadingParameter(Name = nameof(RadioGroupOption<object>))]
    public IAriaCheckGroupOption? RadioGroupOption { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public RadioGroupLabel() : base("label")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", RadioGroup.GetId(this)), () =>
                new KeyValuePair<string, object?>("onclick",
                    RadioGroupOption != null ? EventCallback.Factory.Create(this, RadioGroupOption.FocusAsync) : null),
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
            RadioGroupOption.Label = this;
        }
        else
        {
            RadioGroup.Label = this;
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
