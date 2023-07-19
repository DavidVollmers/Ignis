using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface ITab : IDynamicParentComponent<ITab>, IFocus
{
    bool IsSelected { get; }
}
