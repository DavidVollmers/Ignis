using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface ITransition : IDynamicParentComponent<ITransition>, ICssClass
{
    void Hide(Action? continueWith = null);

    void Show(Action? continueWith = null);
    
    internal void AddChild(ITransitionChild child);
    
    internal void RemoveChild(ITransitionChild child);
}
