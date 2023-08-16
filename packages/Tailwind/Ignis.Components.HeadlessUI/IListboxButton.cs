using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface IListboxButton : IDynamicParentComponent<IListboxButton>
{
    string? Id { get; set; }
}
