using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface ITransitionChild : IDynamicParentComponent<ITransitionChild>, ICssClass
{
    internal void Hide(Action? onHidden = null);

    internal void Show();
}
