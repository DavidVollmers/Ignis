﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class SwitchDescription : IgnisRigidComponentBase, ISwitchDescription
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

    [CascadingParameter] public ISwitchGroup SwitchGroup { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<ISwitchDescription>? _ { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the switch label.
    /// </summary>
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

    public SwitchDescription()
    {
        AsElement = "p";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id ?? SwitchGroup.Id + "-description")
        });
    }

    /// <inheritdoc />
    protected override void OnRender()
    {
        if (SwitchGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(SwitchDescription)} must be used inside a {nameof(HeadlessUI.SwitchGroup)}.");
        }

        SwitchGroup.SetDescription(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<ISwitchDescription, SwitchDescription>(2, this, ChildContent);

        builder.CloseAs(this);
    }
}