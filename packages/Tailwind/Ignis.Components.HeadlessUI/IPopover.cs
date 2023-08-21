using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface IPopover : IDynamicParentComponent<IPopover>, IOpenClose, IWithTransition, IFocus
{
    string Id { get; }

    internal void SetButton(IPopoverButton button);
    
    internal void SetPanel(IDynamicParentComponent panel);
}
