namespace Ignis.Components.HeadlessUI;

public interface ITab : IDynamicParentComponent<ITab>
{
    bool IsSelected { get; }
}
