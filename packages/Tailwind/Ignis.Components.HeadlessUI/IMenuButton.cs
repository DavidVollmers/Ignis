namespace Ignis.Components.HeadlessUI;

public interface IMenuButton : IDynamicParentComponent<IMenuButton>
{
    string? Id { get; set; }
}
