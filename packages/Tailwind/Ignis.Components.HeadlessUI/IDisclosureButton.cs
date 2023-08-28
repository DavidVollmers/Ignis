using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IDisclosureButton : IDynamicParentComponent
{
    EventCallback<IComponentEvent> OnClick { get; set; }
}
