using System.ComponentModel.DataAnnotations;
using BlazorServer.Shared.Components;
using Ignis.Fragments.Abstractions;
using Ignis.Fragments.Abstractions.Builder;

namespace BlazorServer.Data;

[RenderAs(typeof(Component<FormComponent, FormFragmentContext>))]
public class ExampleForm
{
    [Display(Name = "Mail Address")]
    [RenderAs(typeof(Component<InputComponent<string>, InputFragmentContext<string>>))]
    public string MailAddress { get; set; } = null!;

    [Display(Name = "Password")]
    [RenderAs(typeof(Component<InputComponent<string>, InputFragmentContext<string>>))]
    public string Password { get; set; } = null!;
}
