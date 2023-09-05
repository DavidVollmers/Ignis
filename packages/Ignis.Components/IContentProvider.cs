using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IContentProvider
{
    IContentHost Outlet { get; set; }
    
    RenderFragment? Content { get; }
}
