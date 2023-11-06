using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

/// <summary>
/// Provides functionality to an object to be used as a listbox.
/// </summary>
public interface IListbox : IDynamicParentComponent<IListbox>, IOpenClose, IWithTransition, IFocus
{
    internal IListboxOption[] Options { get; }

    internal IListboxOption? ActiveOption { get; }

    internal IListboxLabel? Label { get; }

    internal IListboxButton? Button { get; }

    internal string? OptionsId { get; }

    /// <summary>
    /// Gets the id of the listbox.
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
    /// Selects the given value.
    /// </summary>
    /// <param name="value">The value to select.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    void SelectValue<TValue>(TValue? value);

    internal void SetOptionActive(IListboxOption option, bool isActive);

    internal void AddOption(IListboxOption option);

    internal void RemoveOption(IListboxOption option);

    internal void SetButton(IListboxButton button);

    internal void SetLabel(IListboxLabel label);

    internal void SetOptions(IListboxOptions options);

    internal string? GetOptionId(IListboxOption? option);
}
