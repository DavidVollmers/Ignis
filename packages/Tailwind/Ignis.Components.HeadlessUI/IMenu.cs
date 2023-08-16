using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface IMenu : IDynamicParentComponent<IMenu>, IOpenClose, IWithTransition, IFocus
{
    internal IMenuItem[] Items { get; }

    internal IMenuItem? ActiveItem { get; }

    internal IMenuButton? Button { get; }

    string Id { get; }

    internal void SetButton(IMenuButton button);

    internal void SetItemActive(IMenuItem item, bool isActive);

    internal void AddItem(IMenuItem item);

    internal void RemoveItem(IMenuItem item);
}
