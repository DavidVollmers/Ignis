using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IOutletComponent : IComponent
{
    RenderFragment? OutletContent { get; }

    void SetOutlet(IOutlet outlet);

    void SetFree();
}
