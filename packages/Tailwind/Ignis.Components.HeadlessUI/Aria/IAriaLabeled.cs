namespace Ignis.Components.HeadlessUI.Aria;

public interface IAriaLabeled : IAriaComponentPart
{
    IAriaComponentPart? Label { get; set; }
}
