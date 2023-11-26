namespace Ignis.Components.HeadlessUI.Aria;

public interface IAriaControl : IAriaComponent
{
    IAriaComponentPart? Controlled { get; set; }
}
