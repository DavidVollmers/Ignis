using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxOption<TValue> : IgnisComponentBase, IDynamicComponent, IListboxOption
{
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
    
    public bool IsActive { get; private set; }

    public bool IsSelected => Listbox.IsValueSelected(Value);

    public IReadOnlyDictionary<string, object?> Attributes
    {
        get
        {
            var attributes = new Dictionary<string, object?>
            {
                { "tabindex", -1 },
                { "role", "option" },
                { "aria-selected", IsSelected },
                { "onclick", EventCallback.Factory.Create(this, OnClick) },
                { "onmouseenter", EventCallback.Factory.Create(this, OnMouseEnter) },
                { "onmouseleave", EventCallback.Factory.Create(this, OnMouseLeave) }
            };

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

    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    [Parameter, EditorRequired] public TValue? Value { get; set; }

    [Parameter] public RenderFragment<IListboxOption>? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    public ListboxOption()
    {
        AsElement = "li";
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddContentFor(2, this, ChildContent?.Invoke(this));

        builder.CloseAs(this);
    }

    private void OnClick()
    {
        Listbox.SelectValue(Value);

        Listbox.Close();
    }

    private void OnMouseEnter()
    {
        IsActive = true;

        ForceUpdate();
    }

    private void OnMouseLeave()
    {
        IsActive = false;

        ForceUpdate();
    }
}
