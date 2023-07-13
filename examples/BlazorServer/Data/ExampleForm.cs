using System.ComponentModel;

namespace BlazorServer.Data;

public class ExampleForm
{
    [DisplayName("Mail Address")] public string MailAddress { get; set; } = null!;
}