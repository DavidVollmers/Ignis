using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface ITransition : IDynamicParentComponent<ITransition>, ICssClass, IContentProvider
{
    internal bool HasOutletDialogs { get; }

    void Hide(Action? continueWith = null);

    void Show(Action? continueWith = null);

    internal void AddChild(ITransitionChild child);

    internal void RemoveChild(ITransitionChild child);

    internal void AddDialog(IDialog dialog);

    internal void RemoveDialog(IDialog dialog);
}
