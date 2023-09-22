using Ignis.Fragments.Abstractions.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Abstractions;

public sealed class EmptyFragment<T> : IFragmentBuilder<T> where T : class
{
    public RenderFragment? BuildFragment(T context)
    {
        return null;
    }
}
