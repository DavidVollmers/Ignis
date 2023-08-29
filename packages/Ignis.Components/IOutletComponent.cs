using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IOutletComponent : IComponent
{
    object Identifier { get; }
    
    RenderFragment? OutletContent { get; }

    void SetOutlet(IOutlet outlet);

    void SetFree();
}
