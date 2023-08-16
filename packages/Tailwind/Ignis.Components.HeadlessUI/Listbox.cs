﻿using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

/// <summary>
/// Renders a listbox which can be used to select one or more values.
/// </summary>
/// <typeparam name="TValue">The value type.</typeparam>
public sealed class Listbox<TValue> : OpenCloseWithTransitionComponentBase, IListbox
{
    private readonly IList<IListboxOption> _options = new List<IListboxOption>();

    private IDynamicParentComponent? _optionsComponent;
    private Type? _asComponent;
    private string? _asElement;
    private bool _isOpen;

    /// <inheritdoc />
    protected override IEnumerable<ElementReference> TargetElements
    {
        get
        {
            if (Button?.Element.HasValue == true) yield return Button.Element.Value;

            if (Label?.Element.HasValue == true) yield return Label.Element.Value;

            if (_optionsComponent?.Element.HasValue == true) yield return _optionsComponent.Element.Value;

            foreach (var option in _options)
            {
                if (option.Element.HasValue) yield return option.Element.Value;
            }
        }
    }

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

    /// <summary>
    /// The selected value.
    /// </summary>
    [Parameter]
    public TValue? Value { get; set; }

    /// <summary>
    /// Occurs when the selected value changes.
    /// </summary>
    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IListbox>? _ { get; set; }

    [Parameter] public RenderFragment<IListbox>? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the listbox.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public IListboxOption[] Options => _options.ToArray();

    /// <inheritdoc />
    public IListboxOption? ActiveOption { get; private set; }

    /// <inheritdoc />
    public IListboxLabel? Label { get; private set; }

    /// <inheritdoc />
    public IListboxButton? Button { get; private set; }

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-listbox-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    /// <summary>
    /// Initializes a new instance of the <see cref="Listbox{TValue}"/> class.
    /// </summary>
    public Listbox()
    {
        AsComponent = typeof(Fragment);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddContentFor(2, this, builder =>
        {
            builder.OpenComponent<CascadingValue<IListbox>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<IListbox>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<IListbox>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<IListbox>.ChildContent), this.GetChildContent(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    protected override void OnAfterOpen(Action? continueWith)
    {
        var selectedOption = Options.FirstOrDefault(x => x.IsSelected);
        if (selectedOption != null) SetOptionActive(selectedOption, true);

        base.OnAfterOpen(continueWith);
    }

    /// <inheritdoc />
    public bool IsValueSelected<TValue1>(TValue1? value)
    {
        return value?.Equals(Value) ?? Value?.Equals(value) ?? false;
    }

    /// <inheritdoc />
    public void SelectValue<TValue1>(TValue1? value)
    {
        ValueChanged.InvokeAsync(Value = (TValue?)(object?)value);

        Update();
    }

    /// <inheritdoc />
    public void SetOptionActive(IListboxOption option, bool isActive)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        if (isActive)
        {
            ActiveOption = option;
        }
        else if (ActiveOption == option)
        {
            ActiveOption = null;
        }

        Update();
    }

    /// <inheritdoc />
    public void AddOption(IListboxOption option)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        if (!_options.Contains(option)) _options.Add(option);
    }

    /// <inheritdoc />
    public void RemoveOption(IListboxOption option)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        _options.Remove(option);
    }

    /// <inheritdoc />
    public void SetButton(IListboxButton button)
    {
        Button = button ?? throw new ArgumentNullException(nameof(button));
    }

    /// <inheritdoc />
    public void SetLabel(IListboxLabel label)
    {
        Label = label ?? throw new ArgumentNullException(nameof(label));
    }

    /// <inheritdoc />
    public void SetOptions(IDynamicParentComponent options)
    {
        _optionsComponent = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    protected override void OnBlur()
    {
        Close();
    }
}
