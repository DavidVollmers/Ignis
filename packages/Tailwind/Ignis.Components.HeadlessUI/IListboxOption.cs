using Ignis.Components.HeadlessUI.Aria;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IListboxOption : IDynamicParentComponent<IListboxOption>, IAriaComponentDescendant
{
    bool IsSelected { get; }

    EventCallback<IComponentEvent> OnClick { get; set; }

    internal void Click();
}
