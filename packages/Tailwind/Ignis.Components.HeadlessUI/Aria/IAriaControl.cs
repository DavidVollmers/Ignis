namespace Ignis.Components.HeadlessUI.Aria;

/// <summary>
/// A ARIA conform component that controls another component part.
/// </summary>
public interface IAriaControl : IAriaComponent
{
    /// <summary>
    /// The component part that is controlled by this component.
    /// </summary>
    IAriaComponentPart? Controlled { get; set; }
}
