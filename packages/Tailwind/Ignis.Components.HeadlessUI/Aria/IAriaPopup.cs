using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI.Aria;

public interface IAriaPopup<T> : IAriaPopup where T : IAriaDescendant
{
    new IEnumerable<T> Descendants { get; }

    new T? ActiveDescendant { get; set; }

    void AddDescendant(T descendant);

    void RemoveDescendant(T descendant);
}

// Only needed to cascade a non-generic type to the non-generic parts. (e.g. Button, Label, etc.)
public interface IAriaPopup : IAriaControl, IOpenClose, IWithTransition, IFocus
{
    IEnumerable<IAriaDescendant> Descendants { get; }
    
    IAriaDescendant? ActiveDescendant { get; set; }

    IAriaComponentPart? Button { get; set; }

    IAriaComponentPart? Label { get; set; }
}
