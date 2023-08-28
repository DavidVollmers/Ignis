using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface IDisclosure : IDynamicParentComponent<IDisclosure>, IOpenClose, IWithTransition, IFocus
{
    internal IDisclosurePanel? Panel { get; }
    
    string Id { get; }

    internal void SetPanel(IDisclosurePanel panel);
    
    internal void SetButton(IDisclosureButton button);
}
