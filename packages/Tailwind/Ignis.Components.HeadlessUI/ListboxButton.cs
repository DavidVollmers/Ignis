using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxButton : IgnisComponentBase, IDynamicParentComponent, IFocus
{
    private bool _preventKeyDownDefault;
    private ElementReference? _element;
    private object? _component;
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

    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    /// <inheritdoc />
    [Parameter] public RenderFragment<IDynamicComponent>? _ { get; set; }
    
    [Parameter] public RenderFragment<IDynamicComponent>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public IReadOnlyDictionary<string, object?>? Attributes
    {
        get
        {
            var attributes = new Dictionary<string, object?>
            {
                { "aria-haspopup", "listbox" },
                { "onclick", EventCallback.Factory.Create(this, Listbox.Open) },
                { "onkeydown", _preventKeyDownDefault },
#pragma warning disable CS0618
                { "onkeydown", EventCallback.Factory.Create(this, OnKeyDown) },
#pragma warning restore CS0618
                { "aria-labelledby", Listbox.Id + "-label" },
                { "aria-expanded", Listbox.IsOpen }
            };
            
            if (AsElement == "button") attributes.Add("type", "button");

            // ReSharper disable once InvertIf
            if (AdditionalAttributes != null)
            {
                foreach (var (key, value) in AdditionalAttributes)
                {
                    attributes[key] = value;
                }
            }

            return attributes;
        }
    }

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
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddReferenceCaptureFor(2, this, e => _element = e, c => _component = c);
        builder.AddChildContentFor(3, this, ChildContent);

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

    /// <inheritdoc />
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
