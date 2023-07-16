using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IListbox<TValue> : IOpenClose
{
    TValue? Value { get; set; }

    EventCallback<TValue?> ValueChanged { get; set; }
}
