using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface ITransition : IDynamicParentComponent<ITransition>, ICssClass
{
    void Hide(Action onHidden);

    void Show();
}
