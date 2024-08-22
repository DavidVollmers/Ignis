namespace Ignis.Components.HeadlessUI.Aria;

/// <summary>
/// A component which can be disabled.
/// </summary>
public interface IAriaDisabled
{
    /// <summary>
    /// Whether the component is disabled.
    /// </summary>
    bool IsDisabled { get; set; }
}
