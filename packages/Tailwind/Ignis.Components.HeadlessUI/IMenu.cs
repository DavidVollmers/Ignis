namespace Ignis.Components.HeadlessUI;

public interface IMenu : IDynamicParentComponent<IMenu>, IOpenClose, IWithTransition
{
    string Id { get; }
    
    internal void SetButton()
}
