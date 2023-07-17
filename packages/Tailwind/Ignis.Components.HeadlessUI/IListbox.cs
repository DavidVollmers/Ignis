using Ignis.Components.Web;
using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IListbox : IOpenClose, IFocus
{
    string Id { get; }
    
    object? Value { get; set; }

    EventCallback<object?> ValueChanged { get; set; }
    
    internal void SetLabel(ListboxLabel label);

    internal void SetButton(ListboxButton button);
}
