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

    public static RenderFragment? Form<T>(T model, OnSubmit onSubmit,
        IFragmentBuilder<FormFragmentContext>? defaultBuilder = null) where T : class
    {
        return Form(model, () =>
        {
            onSubmit();
            return Task.CompletedTask;
        }, defaultBuilder);
    }

    public static RenderFragment? Form<T>(T model, OnSubmit<T> onSubmit,
        IFragmentBuilder<FormFragmentContext>? defaultBuilder = null) where T : class
    {
        return Form(model, m =>
        {
            onSubmit(m);
            return Task.CompletedTask;
        }, defaultBuilder);
    }

    public static RenderFragment? Form<T>(T model, OnSubmitAsync onSubmit,
        IFragmentBuilder<FormFragmentContext>? defaultBuilder = null) where T : class
    {
        return Form(model, async _ => { await onSubmit(); }, defaultBuilder);
    }

    public static RenderFragment? Form<T>(T model, OnSubmitAsync<T> onSubmit,
        IFragmentBuilder<FormFragmentContext>? defaultBuilder = null) where T : class
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        var context = new FormFragmentContext(model, async o => { await onSubmit((T)o); })
        {
            Attributes = GetAttributes(model)
        };

        var builder = TryGetFragmentBuilder<FormFragmentContext>(model) ??
                      defaultBuilder ?? new DefaultFormFragmentBuilder();

        return builder.BuildFragment(context);
    }
}
