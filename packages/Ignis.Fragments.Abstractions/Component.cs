using Ignis.Fragments.Abstractions.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Abstractions;

public sealed class Component<TComponent, TContext> : IFragmentBuilder<TContext>
    where TComponent : IComponent where TContext : class
{
    public RenderFragment? BuildFragment(TContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        return builder =>
        {
            builder.OpenComponent<CascadingValue<TContext>>(0);
            builder.SetKey(context);
            builder.AddAttribute(1, nameof(CascadingValue<TContext>.IsFixed), true);
            builder.AddAttribute(2, nameof(CascadingValue<TContext>.Value), context);
            // ReSharper disable once VariableHidesOuterVariable
            builder.AddAttribute(3, nameof(CascadingValue<TContext>.ChildContent), (RenderFragment)(builder =>
            {
                builder.OpenComponent(4, typeof(TComponent));

                builder.CloseComponent();
            }));

            builder.CloseComponent();
        };
    }
}
