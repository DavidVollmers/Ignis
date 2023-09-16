using Ignis.Fragments.Abstractions.Builder;

namespace Ignis.Tests.Fragments;

public class InputTests : TestContext
{
    [Fact]
    public void Input_Value_OnInput()
    {
        const string newValue = "New Test Value";
        var value = "Test Value";

        var fragment = Render(Input(value, s => value = s)!);

        var input = fragment.Find("input");
        Assert.Equal(value, input.GetAttribute("value"));

        input.Change(newValue);
        // We cannot check the input attribute again since there is no render update...
        Assert.Equal(newValue, value);
    }

    [Fact]
    public void Input_Value_OnInput_Exception()
    {
        const string exceptionMessage = "Exception Message";
        const string value = "Test Value";
        const string newValue = "New Test Value";

        var fragment = Render(Input(value, s => throw new Exception(exceptionMessage))!);

        var input = fragment.Find("input");
        Assert.Equal(value, input.GetAttribute("value"));

        //TODO for some reason the exception is not thrown in .NET 6
        try
        {
            input.Change(newValue);
            Assert.Equal(value, input.GetAttribute("value"));
        }
        catch (Exception exception)
        {
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }

    [Fact]
    public void Input_Value_OnInputAsync_Exception()
    {
        const string exceptionMessage = "Exception Message";
        const string value = "Test Value";
        const string newValue = "New Test Value";

#pragma warning disable CS1998
        var fragment = Render(Input(value, async s => throw new Exception(exceptionMessage))!);
#pragma warning restore CS1998

        var input = fragment.Find("input");
        Assert.Equal(value, input.GetAttribute("value"));

        //TODO for some reason the exception is not thrown in .NET 6
        try
        {
            input.Change(newValue);
            Assert.Equal(value, input.GetAttribute("value"));
        }
        catch (Exception exception)
        {
            Assert.Equal(exceptionMessage, exception.Message);
        }
    }

    [Fact]
    public void Input_Expression()
    {
        const string newValue = "New Test Value";
        const string value = "Test Value";
        var model = new TestModel { StringProperty = value };

        var fragment = Render(Input(() => model.StringProperty)!);

        var input = fragment.Find("input");
        Assert.Equal(value, input.GetAttribute("value"));

        input.Change(newValue);
        Assert.Equal(newValue, model.StringProperty);
    }

    [Fact]
    public void Input_PropertyInfo_WithDefaultBuilder()
    {
        const string value = "Test Value";
        var model = new TestModel();
        var propertyInfo = typeof(TestModel).GetProperty(nameof(TestModel.StringProperty))!;
        var defaultBuilder = new TestInputBuilder<string>(value);

        var fragment = Render(Input(model, propertyInfo, defaultBuilder)!);

        var input = fragment.Find("input");
        Assert.Equal(value, input.GetAttribute("value"));
    }

    [Fact]
    public void Input_PropertyInfo_WithInvalidDefaultBuilder()
    {
        var model = new TestModel();
        var propertyInfo = typeof(TestModel).GetProperty(nameof(TestModel.StringProperty))!;
        var defaultBuilder = new InvalidFragmentBuilder();

        var exception = Assert.Throws<ArgumentException>(() => Render(Input(model, propertyInfo, defaultBuilder)!));
        Assert.Equal(
            $"The default builder must implement {typeof(IFragmentBuilder<>).Name}<{typeof(InputFragmentContext<>)}<{propertyInfo.PropertyType.Name}>>. (Parameter 'defaultBuilder')",
            exception.Message);
    }

    [Fact]
    public void Input_PropertyInfo_WithMissingAttributeTestInputBuilder()
    {
        var model = new TestModel();
        var propertyInfo = typeof(TestModel).GetProperty(nameof(TestModel.StringProperty))!;
        var defaultBuilder = new MissingAttributeTestInputBuilder();

        var fragment = Render(Input(model, propertyInfo, defaultBuilder)!);

        var input = fragment.Find("input");
        Assert.Equal("text", input.GetAttribute("type"));
    }
}
