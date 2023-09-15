using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IContentProvider
{
    RenderFragment? Content { get; }

    void HostedBy(IContentHost host);
}
