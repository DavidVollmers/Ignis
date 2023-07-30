using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface ISwitch : IDynamicParentComponent<ISwitch>, IFocus
{
    string? Id { get; set; }

    bool Checked { get; set; }

    EventCallback<bool> CheckedChanged { get; set; }
}
