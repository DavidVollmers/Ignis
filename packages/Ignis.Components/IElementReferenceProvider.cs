using Microsoft.AspNetCore.Components;

namespace Ignis.Components;

public interface IElementReferenceProvider
{
    ElementReference? Element { get; }
}
