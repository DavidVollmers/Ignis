using Ignis.Components.Web;

namespace Ignis.Components.HeadlessUI;

public interface ITransition : ICssClass
{
    IReadOnlyDictionary<string, object?> Attributes { get; }
}
