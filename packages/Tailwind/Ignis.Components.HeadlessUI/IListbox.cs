using Ignis.Components.HeadlessUI.Aria;
using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

//TODO make this a generic interface
/// <summary>
/// Provides functionality to an object to be used as a listbox.
/// </summary>
public interface IListbox : IDynamicParentComponent<IListbox>, IAriaPopup<IListboxOption>, IWithTransition, IFocus
{
    /// <summary>
    /// Checks if the given value is selected.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>`true` if the value is selected; otherwise `false`.</returns>
    bool IsValueSelected<TValue>(TValue? value);

    /// <summary>
    /// Selects the given value.
    /// </summary>
    /// <param name="value">The value to select.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    void SelectValue<TValue>(TValue? value);
}
