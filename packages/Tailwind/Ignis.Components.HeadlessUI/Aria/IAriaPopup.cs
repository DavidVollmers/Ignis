using Ignis.Components.Web;
using Microsoft.JSInterop;

namespace Ignis.Components.HeadlessUI.Aria;

/// <summary>
/// An ARIA conform popup.
/// </summary>
/// <typeparam name="T">The type of popup descendants.</typeparam>
public interface IAriaPopup<T> : IAriaPopup where T : IAriaDescendant
{
    /// <inheritdoc cref="IAriaPopup.Descendants"/>
    new IEnumerable<T> Descendants { get; }

    /// <inheritdoc cref="IAriaPopup.ActiveDescendant"/>
    new T? ActiveDescendant { get; set; }

    /// <summary>
    /// Adds a descendant to the component.
    /// </summary>
    /// <param name="descendant">The descendant to add.</param>
    void AddDescendant(T descendant);

    /// <summary>
    /// Removes a descendant from the component.
    /// </summary>
    /// <param name="descendant">The descendant to remove.</param>
    void RemoveDescendant(T descendant);
}

// Only needed to cascade a non-generic type to the non-generic parts. (e.g. Button, Label, etc.)
/// <summary>
/// An ARIA conform popup.
/// </summary>
public interface IAriaPopup : IAriaControl, IAriaLabeled, IOpenClose, IWithTransition, IFocus
{
    /// <summary>
    /// The descendants of the component.
    /// </summary>
    IEnumerable<IAriaDescendant> Descendants { get; }

    /// <summary>
    /// The active descendant of the component.
    /// </summary>
    IAriaDescendant? ActiveDescendant { get; set; }

    /// <summary>
    /// The button which controls the popup.
    /// </summary>
    IAriaComponentPart? Button { get; set; }

    // ReSharper disable once InconsistentNaming
    internal IJSRuntime JSRuntime { get; }
}
