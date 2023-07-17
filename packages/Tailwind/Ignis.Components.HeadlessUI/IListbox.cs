using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface IListbox : IOpenClose, IFocus
{
    string Id { get; }

    internal void SetLabel(ListboxLabel label);

    internal void SetButton(ListboxButton button);
}
