using System.ComponentModel;
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
/// <typeparam name="T">The value type.</typeparam>
public sealed class Listbox<T> : OpenCloseWithTransitionComponentBase, IDynamicParentComponent<Listbox<T>>,
    IAriaPopup<ListboxOption<T>>
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
    public T? Value { get; set; }

    /// <summary>
    /// Gets or sets the selected values.
    /// </summary>
    [Parameter]
    public T[]? Values { get; set; }

    /// <summary>
    /// Gets or sets the callback which is invoked when the selected value changes.
    /// </summary>
    [Parameter]
    public EventCallback<T?> ValueChanged { get; set; }

    /// <summary>
    /// Gets or sets the callback which is invoked when the selected values changes.
    /// </summary>
    [Parameter]
    public EventCallback<T[]?> ValuesChanged { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<Listbox<T>>? _ { get; set; }

    /// <summary>
    /// Gets or sets the content of the listbox.
    /// </summary>
    [Parameter]
    public RenderFragment<Listbox<T>>? ChildContent { get; set; }

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
    /// Initializes a new instance of the <see cref="Listbox{T}"/> class.
    /// </summary>
    public Listbox()
    {
        AsComponent = typeof(Fragment);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<Listbox<T>>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<Listbox<T>>.IsFixed), value: true);
        builder.AddAttribute(1, nameof(CascadingValue<Listbox<T>>.Name), nameof(Listbox<T>));
        builder.AddAttribute(2, nameof(CascadingValue<Listbox<T>>.Value), this);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddAttribute(3, nameof(CascadingValue<Listbox<T>>.ChildContent), (RenderFragment)(builder =>
        {
            builder.OpenAs(4, this);
            builder.AddMultipleAttributes(5, Attributes!);
            builder.AddChildContentFor(6, this, ChildContent);

            builder.CloseAs(this);
        }));

        builder.CloseComponent();
    }

    #endregion Rendering

    #region ARIA

    private readonly IList<ListboxOption<T>> _descendants = new List<ListboxOption<T>>();

    private ListboxOption<T>? _activeDescendant;

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-listbox-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc />
    public IEnumerable<ListboxOption<T>> Descendants => _descendants;

    IEnumerable<IAriaDescendant> IAriaPopup.Descendants => _descendants;

    /// <inheritdoc />
    public ListboxOption<T>? ActiveDescendant
    {
        get => _activeDescendant;
        set
        {
            _activeDescendant = value;

            Update();
        }
    }

    IAriaDescendant? IAriaPopup.ActiveDescendant
    {
        get => _activeDescendant;
        set => ActiveDescendant = (ListboxOption<T>?)value;
    }

    /// <inheritdoc />
    public IAriaComponentPart? Controlled { get; set; }

    /// <inheritdoc />
    public IAriaComponentPart? Button { get; set; }

    /// <inheritdoc />
    public IAriaComponentPart? Label { get; set; }

    /// <inheritdoc />
    public void AddDescendant(ListboxOption<T> descendant)
    {
        if (descendant == null) throw new ArgumentNullException(nameof(descendant));

        if (!_descendants.Contains(descendant)) _descendants.Add(descendant);
    }

    /// <inheritdoc />
    public void RemoveDescendant(ListboxOption<T> descendant)
    {
        if (descendant == null) throw new ArgumentNullException(nameof(descendant));

        _descendants.Remove(descendant);
    }

    /// <inheritdoc />
    public string? GetId(IAriaComponentPart? componentPart)
    {
        if (componentPart == null) return null;

        if (componentPart.Id != null) return componentPart.Id;

        if (componentPart == Button) return Id + "-button";

        if (componentPart == Label) return Id + "-label";

        if (componentPart == Controlled) return Id + "-controlled";

        if (componentPart is not ListboxOption<T> option) return null;

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
    protected override void OnTargetBlur()
    {
        Close();
    }

    #endregion Focus

    #region Listbox

    public bool Multiple => Values != null;

    public bool IsValueSelected(T value)
    {
        if (Values != null) return Values.Contains(value);
        return value?.Equals(Value) ?? Value?.Equals(value) ?? false;
    }

    public void SelectValue(T value)
    {
        if (Values != null)
        {
            var values = Values.Contains(value)
                ? Values.Where(x => !x!.Equals(value)).ToArray()
                : Values.Append(value).ToArray();

            var __ = ValuesChanged.InvokeAsync(Values = values);
        }
        else
        {
            var __ = ValueChanged.InvokeAsync(Value = value);
        }

        Update();
    }

    /// <inheritdoc />
    protected override void OnAfterOpen(Action? continueWith)
    {
        var selectedOption = _descendants.FirstOrDefault(x => x.IsSelected);
        if (selectedOption != null) this.SetActiveDescendant(selectedOption, isActive: true);

        base.OnAfterOpen(continueWith);
    }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        AriaPopupExtensions.OnKeyDown(this, eventArgs);
    }

    #endregion Listbox
}
