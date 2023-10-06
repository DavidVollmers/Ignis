using Ignis.Components.Extensions;

namespace Ignis.Components.Reactivity;

public sealed class ReactiveLink : ReactiveExpression
{
    private ReactiveLinkExtension? _extension;

    public ReactiveLink(IgnisComponentBase owner) : base(owner)
    {
    }

    protected internal override void Initialize()
    {
        if (_extension != null) return;

        _extension = Owner.GetExtension<ReactiveLinkExtension>();
        if (_extension == null)
            throw new InvalidOperationException(
                "ReactiveLinkExtension not found. Please make sure to call AddIgnisReactivity() in your Program.cs.");

        _extension.AddLink(this);
    }
}
