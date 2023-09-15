namespace Ignis.Components.HeadlessUI;

public interface ISwitchGroup : IDynamicParentComponent<ISwitchGroup>
{
    internal ISwitchDescription? Description { get; }

    internal ISwitchLabel? Label { get; }

    string Id { get; }

    internal void SetSwitch(ISwitch button);

    internal void SetLabel(ISwitchLabel label);

    internal void SetDescription(ISwitchDescription description);

    internal void ToggleSwitch();
}
