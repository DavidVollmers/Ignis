namespace Ignis.Components.HeadlessUI.Aria;

public interface IAriaPopup : IAriaComponent, IOpenClose
{
    IEnumerable<IAriaComponentDescendant> Descendants { get; }

    IAriaComponentDescendant? ActiveDescendant { get; set; }

    IAriaComponentControlled? Controlled { get; set; }

    IAriaComponentButton? Button { get; set; }

    IAriaComponentLabel? Label { get; set; }

    void AddDescendant(IAriaComponentDescendant descendant);

    void RemoveDescendant(IAriaComponentDescendant descendant);
}
