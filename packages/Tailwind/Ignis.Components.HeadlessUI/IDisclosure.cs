namespace Ignis.Components.HeadlessUI;

public interface IDisclosure : IDynamicParentComponent<IDisclosure>, IOpenClose, IWithTransition
{
    internal IDisclosurePanel? Panel { get; }
    
    string Id { get; }

    internal void SetPanel(IDisclosurePanel panel);
}
