using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IListboxOption : IDynamicParentComponent<IListboxOption>
{
    string? Id { get; set; }

    bool IsActive { get; }

    bool IsSelected { get; }

    EventCallback<IComponentEvent> OnClick { get; set; }

    internal void Click();
}
