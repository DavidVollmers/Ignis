using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface ITransition : IDynamicParentComponent<ITransition>, ICssClass
{
    void Hide(Action? onHidden = null);

    void Show();
    
    internal void AddChild(ITransitionChild child);
    
    internal void RemoveChild(ITransitionChild child);
}
