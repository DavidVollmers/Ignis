using Microsoft.AspNetCore.Components;

namespace Ignis.Components.HeadlessUI;

public interface IOpenClose
{
    bool IsOpen { get; set; }

    EventCallback<bool> IsOpenChanged { get; set; }

    void Open(Action? continueWith = null);

    void Close(Action? continueWith = null);
}
