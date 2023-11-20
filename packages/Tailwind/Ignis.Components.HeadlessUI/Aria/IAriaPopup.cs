using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI.Aria;

public interface IAriaPopup<T> : IAriaPopup where T : IAriaComponentDescendant
{
    IEnumerable<T> Descendants { get; }

    new T? ActiveDescendant { get; }

    void AddDescendant(T descendant);

    void RemoveDescendant(T descendant);
}

// Only needed to cascade a non-generic type to the non-generic parts. (e.g. Button, Label, etc.)
public interface IAriaPopup : IAriaComponent, IOpenClose, IFocus
{
    IAriaComponentDescendant? ActiveDescendant { get; }

    IAriaComponentPart? Controlled { get; set; }

    IAriaComponentPart? Button { get; set; }

    IAriaComponentPart? Label { get; set; }
}
