using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxOption<TValue> : IgnisComponentBase, IListboxOption, IDisposable
{
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

    [Parameter, EditorRequired] public TValue? Value { get; set; }

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IListboxOption>? _ { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    public bool IsActive => Listbox.ActiveOption == this;

    /// <inheritdoc />
    public bool IsSelected => Listbox.IsValueSelected(Value);

    /// <inheritdoc />
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

    public ListboxOption()
    {
        AsElement = "li";
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        if (Listbox == null)
        {
            throw new InvalidOperationException(
                $"{nameof(ListboxOption<object>)} must be used inside a {nameof(Listbox<object>)}.");
        }

        Listbox.AddOption(this);
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<IListboxOption, ListboxOption<TValue>>(2, this);

        builder.CloseAs(this);
    }

    private void OnClick()
    {
        Listbox.SelectValue(Value);

        Listbox.Close();
    }

    private void OnMouseEnter()
    {
        Listbox.SetOptionActive(this, true);
    }

    private void OnMouseLeave()
    {
        Listbox.SetOptionActive(this, false);
    }

    /// <inheritdoc />
    public void Select()
    {
        Listbox.SelectValue(Value);
    }

    public void Dispose()
    {
        Listbox.RemoveOption(this);
    }
}
