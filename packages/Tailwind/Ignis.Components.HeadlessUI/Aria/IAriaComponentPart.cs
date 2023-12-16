namespace Ignis.Components.HeadlessUI.Aria;

/// <summary>
/// A part of an ARIA conform component.
/// </summary>
public interface IAriaComponentPart : IElementReferenceProvider
{
    /// <summary>
    /// The id of the component part.
    /// </summary>
    string? Id { get; }
}
