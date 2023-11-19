using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public abstract class IgnisDynamicComponentBase<T> : IgnisComponentBase, IDynamicParentComponent<T>
    where T : IgnisDynamicComponentBase<T>
{
    private readonly AttributeCollection _attributes;

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
        get => _attributes.AdditionalAttributes;
        set => _attributes.AdditionalAttributes = value;
    }

    public ElementReference? Element { get; set; }

    public object? Component { get; set; }

    public IEnumerable<KeyValuePair<string, object?>>? Attributes => _attributes;

    protected IgnisDynamicComponentBase(string asElement, IEnumerable<Func<KeyValuePair<string, object?>>> attributes)
        : this(attributes)
    {
        _asElement = asElement ?? throw new ArgumentNullException(nameof(asElement));
    }

    protected IgnisDynamicComponentBase(Type asComponent, IEnumerable<Func<KeyValuePair<string, object?>>> attributes)
        : this(attributes)
    {
        _asComponent = asComponent ?? throw new ArgumentNullException(nameof(asComponent));
    }

    private protected IgnisDynamicComponentBase(IEnumerable<Func<KeyValuePair<string, object?>>> attributes)
    {
        _attributes = new AttributeCollection(attributes);
    }
}
