namespace Ignis.Components.HeadlessUI;

public interface IRadioGroup : IDynamicParentComponent<IRadioGroup>
{
    internal IRadioGroupOption[] Options { get; }

    internal IRadioGroupOption? ActiveOption { get; }

    /// <summary>
    /// A unique identifier for the radio group.
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Checks if the given value is checked.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <returns>`true` if the value is checked; otherwise `false`.</returns>
    bool IsValueChecked<TValue>(TValue? value);

    /// <summary>
    /// Checks the given value.
    /// </summary>
    /// <param name="value">The value to check.</param>
    /// <typeparam name="TValue">The value type.</typeparam>
    void CheckValue<TValue>(TValue? value);

    internal void SetOptionActive(IRadioGroupOption option, bool isActive);

    internal void AddOption(IRadioGroupOption option);

    internal void RemoveOption(IRadioGroupOption option);

    internal void SetLabel(IRadioGroupLabel label);
}
