using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IDynamicParentComponent<T> : IDynamicComponent where T : IDynamicParentComponent<T>
{
    RenderFragment<T>? _ { get; set; }

    IEnumerable<KeyValuePair<string, object?>>? Attributes { get; }
}
