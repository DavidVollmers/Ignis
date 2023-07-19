using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

/// <summary>
/// Provides functionality to an object to be used as a listbox.
/// </summary>
public interface IListbox : IDynamicParentComponent<IListbox>, IOpenClose, IFocus
{
    internal IListboxOption[] Options { get; }

    internal IListboxOption? ActiveOption { get; }
    
    internal IListboxLabel? Label { get; }

    /// <summary>
    /// A unique identifier for the listbox.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Checks if the given value is selected.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>`true` if the value is selected; otherwise `false`.</returns>
    bool IsValueSelected<TValue>(TValue? value);

    /// <summary>
    /// Select the given value.
    /// </summary>
    /// <param name="value">The value to select.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    void SelectValue<TValue>(TValue? value);

    internal void SetOptionActive(IListboxOption option, bool isActive);

    internal void AddOption(IListboxOption option);

    internal void RemoveOption(IListboxOption option);

    internal void SetButton(IFocus button);

    internal void SetLabel(IListboxLabel label);

    internal void SetTransition(ITransition transition);
}
