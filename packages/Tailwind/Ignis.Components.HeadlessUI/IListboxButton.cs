using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface IListboxButton : IDynamicParentComponent<IListboxButton>, IFocus
{
    string? Id { get; set; }
}
