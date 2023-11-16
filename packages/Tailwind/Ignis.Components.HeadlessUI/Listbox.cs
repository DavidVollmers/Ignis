using System.Globalization;
using Ignis.Components.HeadlessUI.Aria;
using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

/// <summary>
/// Renders a listbox which can be used to select one or more values.
/// </summary>
/// <typeparam name="TValue">The value type.</typeparam>
public sealed class Listbox<TValue> : OpenCloseWithTransitionComponentBase, IListbox
{
    #region Parameters

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
    /// Gets or sets the selected value.
    /// </summary>
    [Parameter]
    public TValue? Value { get; set; }

    /// <summary>
    /// Gets or sets the callback which is invoked when the selected value changes.
    /// </summary>
    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IListbox>? _ { get; set; }

    /// <summary>
    /// Gets or sets the content of the listbox.
    /// </summary>
    [Parameter]
    public RenderFragment<IListbox>? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets additional attributes that will be applied to the listbox.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    #endregion Parameters

    #region Rendering

    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
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

    #endregion Rendering

    #region ARIA

    private readonly IList<IListboxOption> _descendants = new List<IListboxOption>();

    /// <inheritdoc />
    public IEnumerable<IListboxOption> Descendants => _descendants;

    /// <inheritdoc />
    public IListboxOption? ActiveDescendant { get; private set; }

    /// <inheritdoc />
    public IAriaComponentPart? Controlled { get; set; }

    /// <inheritdoc />
    public IAriaComponentPart? Button { get; set; }

    /// <inheritdoc />
    public IAriaComponentPart? Label { get; set; }

    /// <inheritdoc />
    public void AddDescendant(IListboxOption descendant)
    {
        if (descendant == null) throw new ArgumentNullException(nameof(descendant));

        if (!_descendants.Contains(descendant)) _descendants.Add(descendant);
    }

    /// <inheritdoc />
    public void RemoveDescendant(IListboxOption descendant)
    {
        if (descendant == null) throw new ArgumentNullException(nameof(descendant));

        _descendants.Remove(descendant);
    }

    /// <inheritdoc />
    public string? GetId(IAriaComponentPart componentPart)
    {
        if (componentPart == null) throw new ArgumentNullException(nameof(componentPart));

        if (componentPart.Id != null) return componentPart.Id;

        if (componentPart == Button) return Id + "-button";

        if (componentPart == Label) return Id + "-label";

        if (componentPart == Controlled) return Id + "-controlled";

        if (componentPart is not IListboxOption option) return null;

        var index = Array.IndexOf(_descendants.ToArray(), option);
        if (index < 0) return null;

        return Id + "-option-" + index.ToString(CultureInfo.InvariantCulture);
    }

    #endregion ARIA

    #region Focus

    /// <inheritdoc />
    protected override IEnumerable<object> Targets
    {
        get
        {
            if (Button != null) yield return Button;

            if (Label != null) yield return Label;

            if (Controlled != null) yield return Controlled;

            foreach (var option in _descendants)
            {
                yield return option;
            }
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<string> KeysToCapture { get; } =
        new[] { "Escape", "Space", "Enter", "ArrowUp", "ArrowDown" };

    /// <inheritdoc />
    protected override void OnBlur()
    {
        Close();
    }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        switch (eventArgs.Code)
        {
            case "Escape":
                Close();
                break;
            case "Space" or "Enter":
                if (IsOpen)
                {
                    if (ActiveDescendant != null) ActiveDescendant.Click();
                    else Close();
                }
                else
                {
                    Open();
                }

                break;
            case "ArrowUp" when ActiveDescendant == null:
            case "ArrowDown" when ActiveDescendant == null:
                if (_descendants.Any()) SetOptionActive(_descendants[0], isActive: true);
                else if (!IsOpen) Open();
                break;
            case "ArrowDown":
            {
                var index = Array.IndexOf(_descendants.ToArray(), ActiveDescendant) + 1;
                if (index < _descendants.Count) SetOptionActive(_descendants[index], isActive: true);
                else if (!IsOpen) Open();
                break;
            }
            case "ArrowUp":
            {
                var index = Array.IndexOf(_descendants.ToArray(), ActiveDescendant) - 1;
                if (index >= 0) SetOptionActive(_descendants[index], isActive: true);
                else if (!IsOpen) Open();
                break;
            }
        }
    }

    #endregion Focus

    #region Listbox

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-listbox-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc />
    protected override void OnAfterOpen(Action? continueWith)
    {
        var selectedOption = _descendants.FirstOrDefault(x => x.IsSelected);
        if (selectedOption != null) SetOptionActive(selectedOption, isActive: true);

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
        var __ = ValueChanged.InvokeAsync(Value = (TValue?)(object?)value);

        Update();
    }

    private void SetOptionActive(IListboxOption option, bool isActive)
    {
        if (option == null) throw new ArgumentNullException(nameof(option));

        if (isActive)
        {
            ActiveDescendant = option;
        }
        else if (ActiveDescendant == option)
        {
            ActiveDescendant = null;
        }

        Update();
    }

    #endregion Listbox
}
