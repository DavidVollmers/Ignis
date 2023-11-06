namespace Ignis.Components.HeadlessUI.Aria;

public interface IAriaComponent : IAriaComponentPart
{
    string? GetId(IAriaComponentPart componentPart);
}
