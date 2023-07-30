using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface ISwitch : IDynamicParentComponent<ISwitch>
{
    string? Id { get; set; }

    bool Checked { get; set; }

    EventCallback<bool> CheckedChanged { get; set; }
}
