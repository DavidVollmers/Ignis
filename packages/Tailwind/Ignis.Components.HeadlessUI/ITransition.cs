using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface ITransition : IDynamicParentComponent<ITransition>, ICssClass
{
    bool Appear { get; set; }
    
    void Hide(Action onHidden);

    void Show();
}
