using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IListboxButton : IDynamicParentComponent<IListboxButton>
{
    string? Id { get; set; }
    
    EventCallback<IComponentEvent> OnClick { get; set; }
}
