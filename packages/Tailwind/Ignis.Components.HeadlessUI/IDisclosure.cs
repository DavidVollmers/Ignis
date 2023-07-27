namespace Ignis.Components.HeadlessUI;

public interface IDisclosure : IDynamicParentComponent<IDisclosure>, IOpenClose
{
    internal IDisclosurePanel? Panel { get; }
    
    string Id { get; }

    internal void SetPanel(IDisclosurePanel panel);

    internal void SetTransition(ITransition transition);
}
