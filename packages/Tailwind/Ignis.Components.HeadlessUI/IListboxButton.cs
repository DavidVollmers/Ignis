using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IListboxButton : IDynamicParentComponent<IListboxButton>, IAriaComponentPart
{
    EventCallback<IComponentEvent> OnClick { get; set; }
}
