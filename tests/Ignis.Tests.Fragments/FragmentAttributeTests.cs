using Ignis.Fragments.Abstractions;

namespace Ignis.Tests.Fragments;

public class FragmentAttributeTests
{
    [Fact]
    public void FragmentAttribute_MustImplementIFragmentBuilder()
    {
        var exception = Assert.Throws<ArgumentException>(() => new RenderAsAttribute(typeof(InvalidFragmentBuilder)));

        Assert.Equal(
            "All fragment builders must implement the type generic IFragmentBuilder interface. (Parameter 'builderType')",
            exception.Message);
    }
}
