using Ignis.Components.Extensions;

namespace Ignis.Components.Reactivity;

internal class ReactiveReferenceExtension : IComponentExtension
{
    private readonly Dictionary<IgnisComponentBase, List<ReactiveExpression>> _subscriptions = new();

    public void OnUpdate(IgnisComponentBase component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        //TODO check tree
    }

    public void OnDispose(IgnisComponentBase component)
    {
        if (component == null) throw new ArgumentNullException(nameof(component));

        _subscriptions.Remove(component);
    }

    public void Subscribe(ReactiveExpression expression, IgnisComponentBase target)
    {
        if (expression == null) throw new ArgumentNullException(nameof(expression));
        if (target == null) throw new ArgumentNullException(nameof(target));

        if (!_subscriptions.ContainsKey(target))
        {
            _subscriptions.Add(target, new List<ReactiveExpression>());
        }

        _subscriptions[target].Add(expression);
    }
}
