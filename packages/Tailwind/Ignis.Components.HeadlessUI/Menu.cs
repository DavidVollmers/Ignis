using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class Menu : OpenCloseWithTransitionComponentBase, IDynamicParentComponent<Menu>
{
    private readonly IList<MenuItem> _items = new List<MenuItem>();

    private MenuItems? _itemsComponent;
    private Type? _asComponent;
    private string? _asElement;

    /// <inheritdoc />
    protected override IEnumerable<object> Targets
    {
        get
        {
            if (Button != null) yield return Button;

            if (_itemsComponent != null) yield return _itemsComponent;

            foreach (var item in Items)
            {
                yield return item;
            }
        }
    }

    /// <inheritdoc />
    protected override IEnumerable<string> KeysToCapture { get; } =
        new[] { "Escape", "Space", "Enter", "ArrowUp", "ArrowDown" };

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

    public MenuItem? ActiveItem { get; private set; }

    public MenuButton? Button { get; private set; }

    public string Id { get; } = "ignis-hui-menu-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc cref="IElementReferenceProvider.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    /// <inheritdoc />
    public MenuItem[] Items => _items.ToArray();

    public Menu()
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
            builder.OpenComponent<CascadingValue<Menu>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<Menu>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<Menu>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<Menu>.ChildContent), this.GetChildContent(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    public void SetItemActive(MenuItem item, bool isActive)
    {
        if (isActive)
        {
            ActiveItem = item;
        }
        else if (ActiveItem == item)
        {
            ActiveItem = null;
        }

        Update();
    }

    public void AddItem(MenuItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        if (!_items.Contains(item)) _items.Add(item);
    }

    public void RemoveItem(MenuItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        _items.Remove(item);
    }

    public void SetButton(MenuButton button)
    {
        Button = button ?? throw new ArgumentNullException(nameof(button));
    }

    public void SetItems(MenuItems items)
    {
        _itemsComponent = items ?? throw new ArgumentNullException(nameof(items));
    }

    /// <inheritdoc />
    protected override void OnBlur()
    {
        Close();
    }

    /// <inheritdoc />
#pragma warning disable MA0051
    protected override void OnKeyDown(KeyboardEventArgs eventArgs)
#pragma warning restore MA0051
    {
        switch (eventArgs.Code)
        {
            case "Escape":
                Close();
                break;
            case "Space" or "Enter":
                if (IsOpen)
                {
                    if (ActiveItem != null) ActiveItem.Click();
                    else Close();
                }
                else
                {
                    Open(() =>
                    {
                        var firstItem = Items.FirstOrDefault();
                        if (firstItem != null) SetItemActive(firstItem, isActive: true);
                    });
                }

                break;
            case "ArrowUp" when ActiveItem == null:
            case "ArrowDown" when ActiveItem == null:
                if (Items.Any()) SetItemActive(Items[0], isActive: true);
                else if (!IsOpen)
                {
                    Open(() =>
                    {
                        if (Items.Any()) SetItemActive(Items[0], isActive: true);
                    });
                }

                break;
            case "ArrowDown":
                {
                    var index = Array.IndexOf(Items, ActiveItem) + 1;
                    if (index < Items.Length) SetItemActive(Items[index], isActive: true);
                    else if (!IsOpen)
                    {
                        Open(() =>
                        {
                            if (Items.Any()) SetItemActive(Items[0], isActive: true);
                        });
                    }

                    break;
                }
            case "ArrowUp":
                {
                    var index = Array.IndexOf(Items, ActiveItem) - 1;
                    if (index >= 0) SetItemActive(Items[index], isActive: true);
                    else if (!IsOpen)
                    {
                        Open(() =>
                        {
                            if (Items.Any()) SetItemActive(Items[0], isActive: true);
                        });
                    }

                    break;
                }
        }
    }
}
