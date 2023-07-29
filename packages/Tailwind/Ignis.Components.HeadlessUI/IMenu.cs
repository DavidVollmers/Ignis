namespace Ignis.Components.HeadlessUI;

public interface IMenu : IDynamicParentComponent<IMenu>, IOpenClose, IWithTransition
{
    internal IMenuButton? Button { get; }
    
    string Id { get; }

    internal void SetButton(IMenuButton button);
}
