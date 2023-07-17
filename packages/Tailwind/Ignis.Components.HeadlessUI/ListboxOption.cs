using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Ignis.Components.HeadlessUI;

public sealed class ListboxOption<TValue> : IgnisDynamicComponentBase
{
    private bool _isActive;

    [CascadingParameter] public IListbox Listbox { get; set; } = null!;

    [Parameter, EditorRequired] public TValue? Value { get; set; }

    [Parameter] public string? SelectedClass { get; set; }

    [Parameter] public string? ActiveClass { get; set; }

    [Parameter] public string? InactiveClass { get; set; }

    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? Attributes { get; set; }

    private string CssClass
    {
        get
        {
            var originalClassString = Attributes?.ContainsKey("class") == true ? Attributes["class"] : null;
            if (_isActive) return $"{originalClassString} {ActiveClass}".Trim();
            return Listbox.IsValueSelected(Value)
                ? $"{originalClassString} {SelectedClass}".Trim()
                : $"{originalClassString} {InactiveClass}".Trim();
        }
    }

    public ListboxOption()
    {
        AsElement = "li";
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddAttribute(1, "role", "option");
        builder.AddAttribute(2, "onclick", EventCallback.Factory.Create(this, OnClick));
        builder.AddAttribute(3, "onmouseenter", EventCallback.Factory.Create(this, OnMouseEnter));
        builder.AddAttribute(4, "onmouseleave", EventCallback.Factory.Create(this, OnMouseLeave));
        builder.AddAttribute(5, "tabindex", -1);
        builder.AddAttribute(6, "aria-selected", Listbox.IsValueSelected(Value));
        builder.AddMultipleAttributes(7, Attributes!);
        builder.AddAttribute(8, "class", CssClass);
        builder.AddContentFor(9, this, ChildContent);

        builder.CloseAs(this);
    }

    private void OnClick()
    {
        //TODO on click
        
        ForceUpdate();
    }

    private void OnMouseEnter()
    {
        this._isActive = true;

        ForceUpdate();
    }

    private void OnMouseLeave()
    {
        this._isActive = false;

        ForceUpdate();
    }
}
