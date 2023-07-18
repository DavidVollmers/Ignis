using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface IListbox : IOpenClose, IFocus
{
    internal IListboxOption[] Options { get; }
    
    internal IListboxOption? ActiveOption { get; }
    
    string Id { get; }

    bool IsValueSelected<TValue>(TValue? value);

    void SelectValue<TValue>(TValue? value);
    
    internal void SetOptionActive(IListboxOption option, bool isActive);
    
    internal void AddOption(IListboxOption option);

    internal void RemoveOption(IListboxOption option);
    
    internal void SetButton(IFocus button);

    internal void SetTransition(ITransition transition);
}
