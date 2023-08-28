using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IRadioGroupOption : IDynamicParentComponent<IRadioGroupOption>, IFocus
{
    bool IsActive { get;  }
    
    bool IsChecked { get; }
    
    EventCallback<IComponentEvent> OnClick { get; set; }
    
    internal void Check();
    
    internal void SetLabel(IRadioGroupLabel label);

    internal void SetDescription(IRadioGroupDescription description);
}
