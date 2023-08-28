using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IMenuButton : IDynamicParentComponent<IMenuButton>
{
    string? Id { get; set; }
    
    EventCallback<IComponentEvent> OnClick { get; set; }
}
