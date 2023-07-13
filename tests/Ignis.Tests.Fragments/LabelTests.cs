namespace Ignis.Tests.Fragments;

public class LabelTests : TestContext
{
    [Fact]
    public void Label_PropertyInfo()
    {
        var propertyInfo = typeof(TestModel).GetProperty(nameof(TestModel.StringProperty))!;

        var fragment = Render(Label(propertyInfo)!);

        var label = fragment.Find("label");
        Assert.Equal(nameof(TestModel.StringProperty), label.GetAttribute("for"));
        Assert.Equal(nameof(TestModel.StringProperty), label.TextContent);
    }
}