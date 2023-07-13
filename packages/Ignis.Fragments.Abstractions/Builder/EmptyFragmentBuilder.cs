using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Abstractions.Builder;

internal class EmptyFragmentBuilder : IFragmentBuilder
{
    public RenderFragment? BuildFragment(object context)
    {
        return null;
    }
}

internal class EmptyFragmentBuilder<T> : IFragmentBuilder<T> where T : class
{
    public RenderFragment? BuildFragment(T context)
    {
        return null;
    }
}