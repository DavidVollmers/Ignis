using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class MenuButton : IgnisComponentBase, IMenuButton
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

    [CascadingParameter] public IMenu Menu { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IMenuButton>? _ { get; set; }

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

    public MenuButton()
    {
        AsElement = "button";

        //TODO aria-controls
        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("id", Id ?? Menu.Id + "-button"),
            () => new KeyValuePair<string, object?>("aria-haspopup", "true"),
            () => new KeyValuePair<string, object?>("onclick", EventCallback.Factory.Create(this, () => Menu.Open())),
            () => new KeyValuePair<string, object?>("__internal_preventDefault_onkeydown", Menu.IsOpen),
#pragma warning disable CS0618
            () => new KeyValuePair<string, object?>("onkeydown", EventCallback.Factory.Create(this, OnKeyDown)),
#pragma warning restore CS0618
            () => new KeyValuePair<string, object?>("aria-expanded", Menu.IsOpen.ToString().ToLowerInvariant()),
            () => new KeyValuePair<string, object?>("type", AsElement == "button" ? "button" : null),
        });
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Menu == null)
        {
            throw new InvalidOperationException(
                $"{nameof(MenuButton)} must be used inside a {nameof(HeadlessUI.Menu)}.");
        }

        Menu.SetButton(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<IMenuButton, MenuButton>(2, this, ChildContent);

        builder.CloseAs(this);
    }

    private void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        switch (eventArgs.Code)
        {
            case "Escape":
                Menu.Close();
                break;
            case "Space" or "Enter":
                if (Menu.IsOpen)
                {
                    Menu.ActiveItem?.Click();
                    
                    Menu.Close();
                }
                else
                {
                    Menu.Open();
                }

                break;
            case "ArrowUp" when Menu.ActiveItem == null:
            case "ArrowDown" when Menu.ActiveItem == null:
                if (Menu.Items.Any()) Menu.SetItemActive(Menu.Items[0], true);
                Menu.Open();
                break;
            case "ArrowDown":
            {
                var index = Array.IndexOf(Menu.Items, Menu.ActiveItem) + 1;
                if (index < Menu.Items.Length) Menu.SetItemActive(Menu.Items[index], true);
                Menu.Open();
                break;
            }
            case "ArrowUp":
            {
                var index = Array.IndexOf(Menu.Items, Menu.ActiveItem) - 1;
                if (index >= 0) Menu.SetItemActive(Menu.Items[index], true);
                Menu.Open();
                break;
            }
        }
    }
}
