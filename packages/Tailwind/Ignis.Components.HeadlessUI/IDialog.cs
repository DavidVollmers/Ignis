namespace Ignis.Components.HeadlessUI;

public interface IDialog : IDynamicParentComponent<IDialog>, IOpenClose
{
    string Id { get; }
}
