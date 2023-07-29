namespace Ignis.Components.HeadlessUI;

public interface IPopover : IDynamicParentComponent<IPopover>, IOpenClose, IWithTransition
{
    internal IPopoverButton? Button { get; }
    
    string Id { get; }

    internal void SetButton(IPopoverButton button);
}
