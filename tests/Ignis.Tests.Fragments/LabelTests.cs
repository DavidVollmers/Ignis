namespace Ignis.Tests.Fragments;

public class LabelTests : TestContext
{
    [Fact]
    public void Label_PropertyInfo_Default()
    {
        var propertyInfo = typeof(TestModel).GetProperty(nameof(TestModel.StringProperty))!;

        var fragment = Render(Label(propertyInfo)!);

        var label = fragment.Find("label");
        Assert.Equal(nameof(TestModel.StringProperty), label.GetAttribute("for"));
        Assert.Equal(nameof(TestModel.StringProperty), label.TextContent);
    }

    [Fact]
    public void Label_PropertyInfo_Default_WithDisplayNameAttribute()
    {
        var propertyInfo = typeof(TestModel).GetProperty(nameof(TestModel.StringPropertyWithDisplayNameAttribute))!;

        var fragment = Render(Label(propertyInfo)!);

        var label = fragment.Find("label");
        Assert.Equal(nameof(TestModel.StringPropertyWithDisplayNameAttribute), label.GetAttribute("for"));
        Assert.Equal(TestModel.StringPropertyWithDisplayNameAttributeValue, label.TextContent);
    }

    [Fact]
    public void Label_PropertyInfo_Default_WithDisplayAttribute()
    {
        var propertyInfo = typeof(TestModel).GetProperty(nameof(TestModel.StringPropertyWithDisplayAttribute))!;

        var fragment = Render(Label(propertyInfo)!);

        var label = fragment.Find("label");
        Assert.Equal(nameof(TestModel.StringPropertyWithDisplayAttribute), label.GetAttribute("for"));
        Assert.Equal(TestModel.StringPropertyWithDisplayAttributeValue, label.TextContent);
    }

    [Fact]
    public void Label_Expression_Default()
    {
        var model = new TestModel();

        var fragment = Render(Label(() => model.StringProperty)!);

        var label = fragment.Find("label");
        Assert.Equal(nameof(TestModel.StringProperty), label.GetAttribute("for"));
        Assert.Equal(nameof(TestModel.StringProperty), label.TextContent);
    }

    [Fact]
    public void Label_Expression_Default_WithDisplayNameAttribute()
    {
        var model = new TestModel();

        var fragment = Render(Label(() => model.StringPropertyWithDisplayNameAttribute)!);

        var label = fragment.Find("label");
        Assert.Equal(nameof(TestModel.StringPropertyWithDisplayNameAttribute), label.GetAttribute("for"));
        Assert.Equal(TestModel.StringPropertyWithDisplayNameAttributeValue, label.TextContent);
    }

    [Fact]
    public void Label_Expression_Default_WithDisplayAttribute()
    {
        var model = new TestModel();

        var fragment = Render(Label(() => model.StringPropertyWithDisplayAttribute)!);

        var label = fragment.Find("label");
        Assert.Equal(nameof(TestModel.StringPropertyWithDisplayAttribute), label.GetAttribute("for"));
        Assert.Equal(TestModel.StringPropertyWithDisplayAttributeValue, label.TextContent);
    }
}
