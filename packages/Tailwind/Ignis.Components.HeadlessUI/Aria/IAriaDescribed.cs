namespace Ignis.Components.HeadlessUI.Aria;

/// <summary>
/// A component part that has a description.
/// </summary>
public interface IAriaDescribed : IAriaComponentPart
{
    /// <summary>
    /// The description of the component part.
    /// </summary>
    IAriaComponentPart? Description { get; set; }
}
