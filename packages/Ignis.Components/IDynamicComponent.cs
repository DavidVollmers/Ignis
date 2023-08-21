using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IDynamicComponent : IComponent, IElementReferenceProvider
{
    new ElementReference? Element { get; set; }

    object? Component { get; set; }
    
    string? AsElement { get; set; }

    Type? AsComponent { get; set; }
}

public interface IDynamicParentComponent<T> : IDynamicComponent where T : IDynamicComponent
{
    RenderFragment<T>? _ { get; set; }

    IEnumerable<KeyValuePair<string, object?>>? Attributes { get; }
}

public interface IDynamicParentComponent : IDynamicParentComponent<IDynamicComponent>
{
}
