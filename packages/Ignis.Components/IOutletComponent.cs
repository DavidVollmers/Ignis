using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IOutletComponent
{
    RenderFragment? OutletContent { get; }
}
