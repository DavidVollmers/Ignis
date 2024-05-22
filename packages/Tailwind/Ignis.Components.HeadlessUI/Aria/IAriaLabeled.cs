namespace Ignis.Components.HeadlessUI.Aria;

/// <summary>
/// A component part that has a label.
/// </summary>
public interface IAriaLabeled : IAriaComponentPart
{
    /// <summary>
    /// The label of the component part.
    /// </summary>
    IAriaComponentPart? Label { get; set; }
}
