using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IDynamicComponent
{
    string? AsElement { get; set; }

    Type? AsComponent { get; set; }

    IReadOnlyDictionary<string, object?>? Attributes { get; }
}

public interface IDynamicParentComponent<T> : IDynamicComponent where T : IDynamicComponent
{
    RenderFragment<T>? ChildContent { get; set; }
}

public interface IDynamicParentComponent : IDynamicParentComponent<IDynamicComponent>
{
}
