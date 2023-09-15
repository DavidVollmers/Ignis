using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Abstractions.Builder;

public interface IFragmentBuilder
{
    RenderFragment? BuildFragment(object context);
}

public interface IFragmentBuilder<in T> : IFragmentBuilder where T : class
{
    RenderFragment? BuildFragment(T context);

    RenderFragment? IFragmentBuilder.BuildFragment(object context)
    {
        return BuildFragment((context as T)!);
    }
}
