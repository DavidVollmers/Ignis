using Ignis.Fragments.Abstractions.Builder;
using Ignis.Fragments.Builder;
using Microsoft.AspNetCore.Components;

namespace Ignis.Fragments;

public static partial class IgnisFragments
{
    public delegate void OnSubmit();

    public delegate void OnSubmit<in T>(T model) where T : class;

    public delegate Task OnSubmitAsync();

    public delegate Task OnSubmitAsync<in T>(T model) where T : class;

    public static RenderFragment? Form<T>(T model, OnSubmit onSubmit) where T : class
    {
        return Form(model, () =>
        {
            onSubmit();
            return Task.CompletedTask;
        });
    }

    public static RenderFragment? Form<T>(T model, OnSubmit<T> onSubmit) where T : class
    {
        return Form(model, m =>
        {
            onSubmit(m);
            return Task.CompletedTask;
        });
    }

    public static RenderFragment? Form<T>(T model, OnSubmitAsync onSubmit) where T : class
    {
        return Form(model, async _ => { await onSubmit(); });
    }

    public static RenderFragment? Form<T>(T model, OnSubmitAsync<T> onSubmit) where T : class
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var attributes = GetAttributes(model);
        
        var context = new FormFragmentContext(model, async o => { await onSubmit((T)o); }, attributes);

        var builder = TryGetFragmentBuilder<FormFragmentContext>(model) ?? new DefaultFormFragmentBuilder();

        return builder.BuildFragment(context);
    }
}
