---
order: 101
title: Example Usage
category: Fragments
permalink: /components/fragments/usage
---

In this example we will create a simple login form using `Ignis.Fragments`.

Let's say you have a model like this:

```csharp
public class LoginModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Password { get; set; }
}
```

And you want to render a form for it. You can do it like this:

```cshtml
@Form(new LoginModel(), OnSubmit)

@code {
    private void OnSubmit(LoginModel model)
    {
        // Do something with the model
    }
}
```

This will render a form with two inputs and a submit button. The submit button will be disabled if the form is not
valid. If the form is valid, the `OnSubmit` callback will be called with the form as an argument.

Of course, this example does not include any styling. You can add your own styling by adding attributes to your class:

```csharp
public class LoginModel
{
    [Required]
    [Attribute("type", "email")]
    [Attribute("class", "form-control")]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    [Attribute("type", "password")]
    [Attribute("class", "form-control")]
    [StringLength(50, MinimumLength = 3)]
    public string Password { get; set; }
}
```

Or if you want to customize the layout and markup used to render your form, you can do it like this:

```csharp
[RenderAs(typeof(Component<MyLoginForm, FormFragmentContext<LoginModel>>))]
public class LoginModel
{
    [Required]
    [Attribute("type", "email")]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }

    [Required]
    [Attribute("type", "password")]
    [StringLength(50, MinimumLength = 3)]
    public string Password { get; set; }
}
```

And build the layout and markup in your component:

```cshtml
<form @onsubmit="@(() => Context.OnSubmitAsync(Context.Model))">
    @foreach (var property in Context.GetProperties())
    {
        <div class="form-group">
            @Label(property)
            @Input(Context.Model, property)
        </div>
    }
    <div class="form-group">
        <button type="submit" class="btn btn-primary">Submit</button>
    </div>
</form>

@code {
    [CascadingParameter]
    public FormFragmentContext<LoginModel> Context { get; set; }
}
```
