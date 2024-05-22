using System.ComponentModel;
using System.Globalization;
using Ignis.Components.HeadlessUI.Aria;
using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class Menu : OpenCloseWithTransitionComponentBase, IDynamicParentComponent<Menu>, IAriaPopup<MenuItem>
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

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<Menu>? _ { get; set; }

    [Parameter] public RenderFragment<Menu>? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the menu.
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

    public Menu()
    {
        AsComponent = typeof(Fragment);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<CascadingValue<Menu>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<Menu>.IsFixed), value: true);
        builder.AddAttribute(2, nameof(CascadingValue<Menu>.Value), this);
        // ReSharper disable once VariableHidesOuterVariable
        builder.AddAttribute(3, nameof(CascadingValue<Menu>.ChildContent), (RenderFragment)(builder =>
        {
            builder.OpenAs(4, this);
            builder.AddMultipleAttributes(5, Attributes!);
            // ReSharper disable once VariableHidesOuterVariable
            builder.AddChildContentFor(6, this, ChildContent);

            builder.CloseAs(this);
        }));

        builder.CloseComponent();
    }

    #endregion Rendering

    #region ARIA

    private readonly IList<MenuItem> _descendants = new List<MenuItem>();

    private MenuItem? _activeDescendant;

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-menu-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc />
    public IEnumerable<MenuItem> Descendants => _descendants.ToArray();

    IEnumerable<IAriaDescendant> IAriaPopup.Descendants => _descendants;

    /// <inheritdoc />
    public MenuItem? ActiveDescendant
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
        set => ActiveDescendant = (MenuItem?)value;
    }

    /// <inheritdoc />
    public IAriaComponentPart? Controlled { get; set; }

    /// <inheritdoc />
    public IAriaComponentPart? Button { get; set; }

    /// <inheritdoc />
    public IAriaComponentPart? Label { get; set; }

    /// <inheritdoc />
    public void AddDescendant(MenuItem descendant)
    {
        if (descendant == null) throw new ArgumentNullException(nameof(descendant));

        if (!_descendants.Contains(descendant)) _descendants.Add(descendant);
    }

    /// <inheritdoc />
    public void RemoveDescendant(MenuItem descendant)
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

        if (componentPart is not MenuItem item) return null;

        var index = Array.IndexOf(_descendants.ToArray(), item);
        if (index < 0) return null;

        return Id + "-item-" + index.ToString(CultureInfo.InvariantCulture);
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

            foreach (var item in _descendants)
            {
                yield return item;
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

    #region Menu

    /// <inheritdoc />
    protected override void OnAfterOpen(Action? continueWith)
    {
        var selectedOption = _descendants.FirstOrDefault();
        if (selectedOption != null) this.SetActiveDescendant(selectedOption, isActive: true);

        base.OnAfterOpen(continueWith);
    }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        AriaPopupExtensions.OnKeyDown(this, eventArgs);
    }

    #endregion Menu
}
