namespace Ignis.Components.HeadlessUI;

public interface IRadioGroup : IDynamicParentComponent<IRadioGroup>
{
    /// <summary>
    /// A unique identifier for the radio group.
    /// </summary>
    string Id { get; }

    internal void SetLabel(IRadioGroupLabel label);
}
