using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Ignis.Components.HeadlessUI;

public sealed class DisclosureButton : IgnisRigidComponentBase, IDynamicParentComponent
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

    [CascadingParameter] public IDisclosure Disclosure { get; set; } = null!;

    /// <inheritdoc />
    [Parameter]
    public RenderFragment<IDynamicComponent>? _ { get; set; }

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

    public DisclosureButton()
    {
        AsElement = "button";

        _attributes = new AttributeCollection(new[]
        {
            () => new KeyValuePair<string, object?>("onclick",
                EventCallback.Factory.Create(this, () => Disclosure.Open())),
#pragma warning disable CS0618
            () => new KeyValuePair<string, object?>("onkeydown", EventCallback.Factory.Create(this, OnKeyDown)),
#pragma warning restore CS0618
            () => new KeyValuePair<string, object?>("aria-expanded", Disclosure.IsOpen),
            () => new KeyValuePair<string, object?>("type", AsElement == "button" ? "button" : null), () =>
                new KeyValuePair<string, object?>("aria-controls",
                    Disclosure.Panel == null ? null : Disclosure.Panel.Id ?? Disclosure.Id + "-panel")
        });
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenAs(0, this);
        builder.AddMultipleAttributes(1, Attributes!);
        builder.AddChildContentFor<IDynamicComponent, DisclosureButton>(3, this, ChildContent);

        builder.CloseAs(this);
    }

    private void OnKeyDown(KeyboardEventArgs eventArgs)
    {
        if (eventArgs.Code is not ("Space" or "Enter")) return;

        if (Disclosure.IsOpen) Disclosure.Close();
        else Disclosure.Open();
    }
}
