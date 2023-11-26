namespace Ignis.Components.HeadlessUI.Aria;

public interface IAriaDescribed : IAriaComponentPart
{
    IAriaComponentPart? Description { get; set; }
}
