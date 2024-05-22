namespace Ignis.Components.HeadlessUI.Aria;

/// <summary>
/// An ARIA conform component.
/// </summary>
public interface IAriaComponent : IAriaComponentPart
{
    /// <summary>
    /// Gets the id of a component part.
    /// </summary>
    /// <param name="componentPart">The component part to get the id of.</param>
    /// <returns>The id of the component part.</returns>
    string? GetId(IAriaComponentPart? componentPart);
}
