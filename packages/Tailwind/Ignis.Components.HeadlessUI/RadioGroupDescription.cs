﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroupDescription : DynamicComponentBase<RadioGroupDescription>
{
    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter] public RadioGroup<object> RadioGroup { get; set; } = null!;

    [CascadingParameter] public RadioGroupOption<object>? RadioGroupOption { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public RadioGroupDescription() : base("div")
    {
        SetAttributes(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id ?? RadioGroup.Id + "-description"),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (RadioGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(RadioGroupDescription)} must be used inside a {nameof(RadioGroup<object>)}.");
        }

        RadioGroupOption?.SetDescription(this);
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
