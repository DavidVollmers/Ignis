using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IMenuItem : IDynamicParentComponent<IMenuItem>
{
    bool IsActive { get; }
    
    EventCallback<IComponentEvent> OnClick { get; set; }

    internal void Click();
}
