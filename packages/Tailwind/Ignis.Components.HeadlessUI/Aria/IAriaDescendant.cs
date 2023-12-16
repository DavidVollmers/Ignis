namespace Ignis.Components.HeadlessUI.Aria;

/// <summary>
/// A component part which is a descendant of an ARIA conform component.
/// </summary>
public interface IAriaDescendant : IAriaComponentPart
{
    /// <summary>
    /// Whether the component part is an active descendant.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Whether the component part is disabled.
    /// </summary>
    void Click();
}
