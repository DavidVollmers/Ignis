using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IDynamicComponent
{
    string? AsElement { get; set; }

    Type? AsComponent { get; set; }
}

public interface IDynamicParentComponent<T> : IDynamicComponent where T : IDynamicComponent
{
    IReadOnlyDictionary<string, object?>? Attributes { get; }
    
    RenderFragment<T>? ChildContent { get; set; }
}

public interface IDynamicParentComponent : IDynamicParentComponent<IDynamicComponent>
{
}
