using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IMenuItem : IDynamicParentComponent<IMenuItem>
{
    bool IsActive { get; }
    
    EventCallback OnClick { get; set; }

    internal void Click();
}
