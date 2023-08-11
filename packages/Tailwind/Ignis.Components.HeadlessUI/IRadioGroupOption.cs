using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface IRadioGroupOption : IDynamicParentComponent<IRadioGroupOption>, IFocus
{
    internal void SetLabel(IRadioGroupLabel label);
}
