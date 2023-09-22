using System.ComponentModel.DataAnnotations;

namespace Ignis.Tests.E2E.Website.Models;

public sealed record WrappedCustomDialogWithFormModel
{
    [Required] public string TestInput { get; set; } = null!;
}
