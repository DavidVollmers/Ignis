using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface ITransition : IDynamicParentComponent<ITransition>, ICssClass
{
    // required for outlet components to render within a transition (e.g. Dialog)
    internal RenderFragment RenderFragment { get; }
    
    void Hide(Action? continueWith = null);

    void Show(Action? continueWith = null);
    
    internal void AddChild(ITransitionChild child);
    
    internal void RemoveChild(ITransitionChild child);
    
    internal void AddDialog(IDialog dialog);
    
    internal void RemoveDialog(IDialog dialog);
}
