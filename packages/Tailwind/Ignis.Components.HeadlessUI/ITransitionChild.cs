using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface ITransitionChild : IDynamicParentComponent<ITransitionChild>, ICssClass
{
    internal void Hide(Action? continueWith = null);

    internal void Show(Action? continueWith = null);
}
