using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IOutletComponent : IComponent, IContentProvider
{
    internal void SetOutlet(IOutlet outlet);

    internal void SetFree();
}
