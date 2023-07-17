using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IListbox<TValue> : IOpenClose
{
    string Id { get; }
    
    TValue? Value { get; set; }

    EventCallback<TValue?> ValueChanged { get; set; }

    Task FocusAsync();
    
    internal void SetLabel(ListboxLabel<TValue> label);

    internal void SetButton(ListboxButton<TValue> button);
}
