using Ignis.Components.Extensions;

namespace Ignis.Components.Reactivity;

public sealed class ReactiveLink : ReactiveExpression
{
    public ReactiveLink(IgnisComponentBase owner) : base(owner)
    {
        var extension = owner.GetExtension<ReactiveLinkExtension>();
        if (extension == null)
            throw new InvalidOperationException(
                "ReactiveLinkExtension not found. Please make sure to call AddIgnisReactivity() in your Program.cs.");

        extension.AddLink(this);
    }
}
