using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class Menu : OpenCloseWithTransitionComponentBase, IMenu
{
    private readonly IList<IMenuItem> _items = new List<IMenuItem>();

    private IDynamicParentComponent? _itemsComponent;
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
    public RenderFragment<IMenu>? _ { get; set; }

    [Parameter] public RenderFragment<IMenu>? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the menu.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public IMenuItem? ActiveItem { get; private set; }

    /// <inheritdoc />
    public IMenuButton? Button { get; private set; }

    /// <inheritdoc />
    public string Id { get; } = "ignis-hui-menu-" + Guid.NewGuid().ToString("N");

    /// <inheritdoc cref="IDynamicParentComponent{T}.Element" />
    public ElementReference? Element { get; set; }

    /// <inheritdoc />
    public object? Component { get; set; }

    /// <inheritdoc />
    public IEnumerable<KeyValuePair<string, object?>>? Attributes => AdditionalAttributes;

    /// <inheritdoc />
    public IMenuItem[] Items => _items.ToArray();

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
            builder.OpenComponent<CascadingValue<IMenu>>(3);
            builder.AddAttribute(4, nameof(CascadingValue<IMenu>.IsFixed), true);
            builder.AddAttribute(5, nameof(CascadingValue<IMenu>.Value), this);
            builder.AddAttribute(6, nameof(CascadingValue<IMenu>.ChildContent), this.GetChildContent(ChildContent));

            builder.CloseComponent();
        });

        builder.CloseAs(this);
    }

    /// <inheritdoc />
    public void SetItemActive(IMenuItem item, bool isActive)
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

    /// <inheritdoc />
    public void AddItem(IMenuItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        if (!_items.Contains(item)) _items.Add(item);
    }

    /// <inheritdoc />
    public void RemoveItem(IMenuItem item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));

        _items.Remove(item);
    }

    /// <inheritdoc />
    public void SetButton(IMenuButton button)
    {
        Button = button ?? throw new ArgumentNullException(nameof(button));
    }

    /// <inheritdoc />
    public void SetItems(IDynamicParentComponent items)
    {
        _itemsComponent = items ?? throw new ArgumentNullException(nameof(items));
    }

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
                    ActiveItem?.Click();
                    Close();
                }
                else
                {
                    Open(() =>
                    {
                        var firstItem = Items.FirstOrDefault();
                        if (firstItem != null) SetItemActive(firstItem, true);
                    });
                }

                break;
            case "ArrowUp" when ActiveItem == null:
            case "ArrowDown" when ActiveItem == null:
                if (Items.Any()) SetItemActive(Items[0], true);
                else if (!IsOpen)
                {
                    Open(() =>
                    {
                        if (Items.Any()) SetItemActive(Items[0], true);
                    });
                }

                break;
            case "ArrowDown":
            {
                var index = Array.IndexOf(Items, ActiveItem) + 1;
                if (index < Items.Length) SetItemActive(Items[index], true);
                else if (!IsOpen)
                {
                    Open(() =>
                    {
                        if (Items.Any()) SetItemActive(Items[0], true);
                    });
                }

                break;
            }
            case "ArrowUp":
            {
                var index = Array.IndexOf(Items, ActiveItem) - 1;
                if (index >= 0) SetItemActive(Items[index], true);
                else if (!IsOpen)
                {
                    Open(() =>
                    {
                        if (Items.Any()) SetItemActive(Items[0], true);
                    });
                }

                break;
            }
        }
    }
}
