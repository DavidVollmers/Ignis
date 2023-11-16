namespace Ignis.Components.HeadlessUI.Aria;

public interface IAriaPopup<T> : IAriaComponent, IOpenClose where T : IAriaComponentDescendant
{
    IEnumerable<T> Descendants { get; }

    T? ActiveDescendant { get; }

    IAriaComponentPart? Controlled { get; set; }

    IAriaComponentPart? Button { get; set; }

    IAriaComponentPart? Label { get; set; }

    void AddDescendant(T descendant);

    void RemoveDescendant(T descendant);
}
