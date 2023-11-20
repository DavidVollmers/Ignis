namespace Ignis.Components.HeadlessUI.Aria;

public interface IAriaModal : IAriaComponent, IOpenClose
{
    IAriaComponentPart? Label { get; set; }

    IAriaComponentPart? Description { get; set; }
}
