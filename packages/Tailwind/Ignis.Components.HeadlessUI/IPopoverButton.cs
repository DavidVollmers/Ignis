using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IPopoverButton : IDynamicParentComponent<IPopoverButton>
{
    EventCallback<IComponentEvent> OnClick { get; set; }
}
