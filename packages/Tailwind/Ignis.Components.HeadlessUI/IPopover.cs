namespace Ignis.Components.HeadlessUI;

public interface IPopover : IDynamicParentComponent<IPopover>, IOpenClose, IWithTransition
{
    string Id { get; }

    internal void SetButton(IPopoverButton button);
}
