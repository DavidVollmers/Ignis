using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public abstract class DynamicComponentBase<T> : IgnisComponentBase, IDynamicParentComponent<T>
    where T : DynamicComponentBase<T>
{
    private const string AttributesNotSetExceptionMessage =
        "Dynamic component attributes not set. Please use the SetAttributes method in the component's constructor.";

    private AttributeCollection? _attributes;

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

    [Parameter] public RenderFragment<T>? _ { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public IEnumerable<KeyValuePair<string, object?>>? AdditionalAttributes
    {
        get => _attributes?.AdditionalAttributes;
        set
        {
            if (_attributes == null) throw new InvalidOperationException(AttributesNotSetExceptionMessage);

            _attributes.AdditionalAttributes = value;
        }
    }

    public ElementReference? Element { get; set; }

    public object? Component { get; set; }

    public IEnumerable<KeyValuePair<string, object?>>? Attributes => _attributes;

    protected DynamicComponentBase(string asElement)
    {
        _asElement = asElement ?? throw new ArgumentNullException(nameof(asElement));
    }

    protected DynamicComponentBase(Type asComponent)
    {
        _asComponent = asComponent ?? throw new ArgumentNullException(nameof(asComponent));
    }

    protected void SetAttributes(IEnumerable<Func<KeyValuePair<string, object?>>> attributes)
    {
        if (attributes == null) throw new ArgumentNullException(nameof(attributes));

        if (_attributes != null) throw new InvalidOperationException("Attributes already set.");

        _attributes = new AttributeCollection(attributes);
    }

    internal override Task OnInitializedCoreAsync()
    {
        if (_attributes == null) throw new InvalidOperationException(AttributesNotSetExceptionMessage);

        return base.OnInitializedCoreAsync();
    }
}
