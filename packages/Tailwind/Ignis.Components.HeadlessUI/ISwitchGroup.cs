using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface ISwitchGroup : IDynamicParentComponent<ISwitchGroup>, IFocus
{
    internal ISwitchDescription? Description { get; }
    
    internal ISwitchLabel? Label { get; }
    
    internal IFocus? Switch { get; }
    
    string Id { get; }
    
    internal void SetSwitch(IFocus button);

    internal void SetLabel(ISwitchLabel label);

    internal void SetDescription(ISwitchDescription description);
}
