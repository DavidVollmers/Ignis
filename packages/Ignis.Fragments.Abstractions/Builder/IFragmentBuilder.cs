using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments.Abstractions.Builder;

public interface IFragmentBuilder<in T>
{
    RenderFragment BuildFragment(T context);
}