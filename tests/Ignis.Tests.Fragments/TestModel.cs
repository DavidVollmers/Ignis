using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Ignis.Tests.Fragments;

public class TestModel
{
    public const string StringPropertyWithDisplayNameAttributeValue =
        "String Property With DisplayName Attribute Value";

    public const string StringPropertyWithDisplayAttributeValue =
        "String Property With Display Attribute Value";

    public string? StringProperty { get; set; }

    [DisplayName(StringPropertyWithDisplayNameAttributeValue)]
    public string? StringPropertyWithDisplayNameAttribute { get; set; }

    [Display(Name = StringPropertyWithDisplayAttributeValue)]
    public string? StringPropertyWithDisplayAttribute { get; set; }
}
