using Ignis.Components.Extensions;

namespace Ignis.Components.Reactivity;

public sealed class ReactiveReference<T> : ReactiveExpression where T : class
{
    private readonly Func<T> _reference;

    public ReactiveReference(IgnisComponentBase owner, Func<T> reference) : base(owner)
    {
        _reference = reference ?? throw new ArgumentNullException(nameof(reference));

        var reactivity = owner.GetExtension<ReactiveReferenceExtension>();
        if (reactivity == null)
            throw new InvalidOperationException(
                "Reactivity extension not found. Please make sure to call AddIgnisReactivity() in your Program.cs.");

        reactivity.Subscribe(this, owner);
    }
    
    //TODO subscribe to reactivty on sections
}
