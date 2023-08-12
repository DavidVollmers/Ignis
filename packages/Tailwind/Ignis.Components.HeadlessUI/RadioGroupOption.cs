﻿using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class RadioGroupOption<TValue> : IgnisComponentBase, IRadioGroupOption, IDisposable
{
    private readonly AttributeCollection _attributes;

    private IRadioGroupLabel? _label;
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

    [Parameter] public TValue? Value { get; set; }

    /// <inheritdoc />
    [Parameter]
    public string? Id { get; set; }

    [CascadingParameter] public IRadioGroup RadioGroup { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IRadioGroupOption>? _ { get; set; }

    [Parameter] public RenderFragment<IRadioGroupOption>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    /// <inheritdoc />
    public bool IsActive => RadioGroup.ActiveOption == this;

    /// <inheritdoc />
    public bool IsChecked => RadioGroup.IsValueChecked(Value);

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>> Attributes => _attributes;

    public RadioGroupOption()
    {
        AsElement = "div";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("role", "radio"),
            () => new KeyValuePair<string, object?>("tabindex", IsChecked ? 0 : -1),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, Check)),
            //TODO track on RadioGroup
            // () => new KeyValuePair<string, object?>("__internal_preventDefault_onkeydown", IsActive),
#pragma warning disable CS0618
            () => new KeyValuePair<string, object?>("onkeydown", EventCallback.Factory.Create(this, OnKeyDown)),
#pragma warning restore CS0618
            () => new KeyValuePair<string, object?>("aria-checked", IsChecked.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("aria-labelledby", _label?.Id)
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (RadioGroup == null)
        {
            throw new InvalidOperationException(
                $"{nameof(RadioGroupOption<TValue>)} must be used inside a {nameof(RadioGroup<object>)}.");
        }

        RadioGroup.AddOption(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        if (AsElement != null) builder.AddElementReferenceCapture(2, e => Element = e);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(3, this, builder =>
        {
            builder.OpenComponent<FocusDetector>(4);
            builder.AddAttribute(5, nameof(FocusDetector.Id), GetId());
            builder.AddAttribute(6, nameof(FocusDetector.OnBlur), EventCallback.Factory.Create(this, () => RadioGroup.SetOptionActive(this, false)));
            builder.AddAttribute(7, nameof(FocusDetector.OnFocus), EventCallback.Factory.Create(this, () => RadioGroup.SetOptionActive(this, true)));
            // ReSharper disable once VariableHidesOuterVariable
            builder.AddAttribute(8, nameof(FocusDetector.ChildContent), (RenderFragment)(builder =>
            {
                builder.OpenComponent<CascadingValue<IRadioGroupOption>>(9);
                builder.AddAttribute(10, nameof(CascadingValue<IRadioGroupOption>.IsFixed), true);
                builder.AddAttribute(11, nameof(CascadingValue<IRadioGroupOption>.Value), this);
                builder.AddAttribute(12, nameof(CascadingValue<IRadioGroupOption>.ChildContent),
                    this.GetChildContent(ChildContent));

                builder.CloseComponent();
            }));

            builder.CloseComponent();
        });
        
        if (AsComponent != null && AsComponent != typeof(Fragment))
            builder.AddComponentReferenceCapture(4, c => Component = c);

        builder.CloseAs(this);
    }

    private string GetId()
    {
        if (Id != null) return Id;
        
        return RadioGroup.Id + "-option-" + Array.IndexOf(RadioGroup.Options, this);
    }

    private void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        if (!RadioGroup.Options.Any()) return;
        
        switch (eventArgs.Code)
        {
            case "ArrowUp" when RadioGroup.ActiveOption == null:
            case "ArrowDown" when RadioGroup.ActiveOption == null:
                RadioGroup.Options[0].Check();
                break;
            case "ArrowDown":
            {
                var index = Array.IndexOf(RadioGroup.Options, RadioGroup.ActiveOption) + 1;
                if (index < RadioGroup.Options.Length) RadioGroup.Options[index].Check();
                else RadioGroup.Options[0].Check();
                break;
            }
            case "ArrowUp":
            {
                var index = Array.IndexOf(RadioGroup.Options, RadioGroup.ActiveOption) - 1;
                if (index >= 0) RadioGroup.Options[index].Check();
                else RadioGroup.Options[^1].Check();
                break;
            }
        }
    }

    /// <inheritdoc />
    public void Check()
    {
        RadioGroup.CheckValue(Value);
        
        RadioGroup.SetOptionActive(this, true);
    }

    /// <inheritdoc />
    public void SetLabel(IRadioGroupLabel label)
    {
        _label = label ?? throw new ArgumentNullException(nameof(label));
    }

    /// <inheritdoc />
    public async Task FocusAsync()
    {
        if (Element.HasValue)
        {
            await Element.Value.FocusAsync();
        }
        else if (Component is IFocus focus)
        {
            await focus.FocusAsync();
        }

        Update();
    }
    
    public void Dispose()
    {
        RadioGroup.RemoveOption(this);
    }
}
