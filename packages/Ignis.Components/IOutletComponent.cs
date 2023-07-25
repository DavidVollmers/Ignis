using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IOutletComponent : IComponent
{
    bool IsAdopted { get; }
    
    bool IgnoreOutlet { get; set; }
    
    RenderFragment? OutletContent { get; }
}
