using Ignis.Fragments.Abstractions.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Tests.Fragments;

public class InvalidFragmentBuilder : IFragmentBuilder
{
    public RenderFragment? BuildFragment(object context)
    {
        throw new NotImplementedException();
    }
}
