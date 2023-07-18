using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface IListbox : IOpenClose, IFocus
{
    string Id { get; }

    bool IsValueSelected<TValue>(TValue? value);

    void SelectValue<TValue>(TValue? value);
    
    internal void SetButton(IFocus button);

    internal void SetTransition(ITransition transition);
}
