﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroupLabel : IgnisRigidComponentBase, IRadioGroupLabel
{
    private readonly AttributeCollection _attributes;

    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    [Parameter]
    public string? AsElement
    {
        get => _asElement;
        set
        {
            _asElement = value;
            _asComponent = null;
        }
    }

    /// <inheritdoc />
    [Parameter]
    public Type? AsComponent
    {
        get => _asComponent;
        set
        {
            _asComponent = value;
            _asElement = null;
        }
    }

    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter] public IRadioGroup RadioGroup { get; set; } = null!;

    [CascadingParameter] public IRadioGroupOption? RadioGroupOption { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IRadioGroupLabel>? _ { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public RadioGroupLabel()
    {
        AsElement = "label";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id ?? RadioGroup.Id + "-label"), () =>
                new KeyValuePair<string, object?>("onclick",
                    RadioGroupOption != null ? EventCallback.Factory.Create(this, RadioGroupOption.FocusAsync) : null)
        });
    }

    /// <inheritdoc />
    protected override void OnRender()
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
        builder.AddChildContentFor<IRadioGroupLabel, RadioGroupLabel>(2, this, ChildContent);

        builder.CloseAs(this);
    }
}