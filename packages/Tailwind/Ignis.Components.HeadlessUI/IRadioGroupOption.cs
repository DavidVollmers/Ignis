using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface IRadioGroupOption : IDynamicParentComponent<IRadioGroupOption>, IFocus
{
    bool IsChecked { get; }
    
    internal void SetLabel(IRadioGroupLabel label);
}
