using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxButton : IgnisComponentBase, IDynamicComponent, IFocus
{
    private bool _preventKeyDownDefault;
    private ElementReference? _element;
    private object? _component;
    private Type? _asComponent;
    private string? _asElement;

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

    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    public ListboxButton()
    {
        AsElement = "button";
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException(
                $"{nameof(ListboxButton)} must be used inside a {nameof(Listbox<object>)}.");
        }

        Listbox.SetButton(this);
    }

    //TODO aria-controls
    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        if (AsElement == "button") builder.AddAttribute(1, "type", "button");
        builder.AddAttribute(2, "aria-haspopup", "listbox");
        builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(this, Listbox.Open));
        builder.AddEventPreventDefaultAttribute(4, "onkeydown", _preventKeyDownDefault);
#pragma warning disable CS0618
        builder.AddAttribute(5, "onkeydown", EventCallback.Factory.Create(this, OnKeyDown));
#pragma warning restore CS0618
        builder.AddAttribute(6, "aria-labelledby", Listbox.Id + "-label");
        builder.AddAttribute(7, "aria-expanded", Listbox.IsOpen);
        builder.AddMultipleAttributes(8, AdditionalAttributes!);
        builder.AddReferenceCaptureFor(9, this, e => _element = e, c => _component = c);
        builder.AddContent(10, ChildContent);

        builder.CloseAs(this);
    }

    private void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        var oldPreventKeyDownDefault = _preventKeyDownDefault;

        _preventKeyDownDefault = true;

        switch (eventArgs.Code)
        {
            case "Escape":
                Listbox.Close();
                break;
            case "Space" or "Enter":
                if (Listbox.IsOpen)
                {
                    Listbox.ActiveOption?.Select();
                    Listbox.Close();
                }
                else
                {
                    Listbox.Open();
                }
                break;
            case "ArrowUp" when Listbox.ActiveOption == null:
            case "ArrowDown" when Listbox.ActiveOption == null:
                if (Listbox.Options.Any()) Listbox.SetOptionActive(Listbox.Options[0], true);
                Listbox.Open();
                break;
            case "ArrowDown":
            {
                var index = Array.IndexOf(Listbox.Options, Listbox.ActiveOption) + 1;
                if (index < Listbox.Options.Length) Listbox.SetOptionActive(Listbox.Options[index], true);
                Listbox.Open();
                break;
            }
            case "ArrowUp":
            {
                var index = Array.IndexOf(Listbox.Options, Listbox.ActiveOption) - 1;
                if (index >= 0) Listbox.SetOptionActive(Listbox.Options[index], true);
                Listbox.Open();
                break;
            }
            default:
                _preventKeyDownDefault = false;
                break;
        }

        if (oldPreventKeyDownDefault != _preventKeyDownDefault)
        {
            ForceUpdate();
        }
    }

    public async Task FocusAsync()
    {
        if (_element.HasValue)
        {
            await _element.Value.FocusAsync();
        }
        else if (_component is IFocus focus)
        {
            await focus.FocusAsync();
        }
    }
}
