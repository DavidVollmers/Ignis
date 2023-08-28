using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface ITab : IDynamicParentComponent<ITab>, IFocus
{
    bool IsSelected { get; }
    
    EventCallback<IComponentEvent> OnClick { get; set; }

    internal void Click();
}
