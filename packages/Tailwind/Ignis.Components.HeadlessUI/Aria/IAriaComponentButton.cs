using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI.Aria;

public interface IAriaComponentButton : IAriaComponentPart
{
    EventCallback<IComponentEvent> OnClick { get; set; }
}
