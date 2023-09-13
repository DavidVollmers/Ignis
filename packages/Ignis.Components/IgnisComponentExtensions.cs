using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ignis.Components;

public static class IgnisComponentExtensions
{
    public static IServiceCollection AddIgnis(this IServiceCollection serviceCollection)
    {
        if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

        serviceCollection.AddTransient<FrameTracker>();

        serviceCollection.TryAddScoped<IContentRegistry, ContentRegistry>();

        return serviceCollection;
    }

#pragma warning disable ASP0006
    public static void OpenAs(this RenderTreeBuilder builder, int sequence, IDynamicComponent dynamicComponent)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        switch (dynamicComponent)
        {
            case null:
                throw new ArgumentNullException(nameof(dynamicComponent));
            case { AsElement: not null, AsComponent: not null }:
                throw new InvalidOperationException(
                    $"Cannot specify both {nameof(IDynamicComponent.AsElement)} and {nameof(IDynamicComponent.AsComponent)} for {dynamicComponent.GetType().Name}.");
            case { AsElement: null, AsComponent: null }:
                throw new InvalidOperationException(
                    $"Must specify either {nameof(IDynamicComponent.AsElement)} and {nameof(IDynamicComponent.AsComponent)} for {dynamicComponent.GetType().Name}.");
        }

        if (dynamicComponent.AsElement != null)
        {
            builder.OpenElement(sequence, dynamicComponent.AsElement);
        }
        else if (dynamicComponent.AsComponent != null)
        {
            if (dynamicComponent.AsComponent.GetInterface(nameof(IComponent)) == null)
            {
                throw new InvalidOperationException(
                    $"Invalid component type {dynamicComponent.AsComponent.Name}. Must implement {nameof(IComponent)}.");
            }

            builder.OpenComponent(sequence, dynamicComponent.AsComponent);
        }
    }
#pragma warning restore ASP0006

    public static void CloseAs(this RenderTreeBuilder builder, IDynamicComponent dynamicComponent)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        switch (dynamicComponent)
        {
            case null:
                throw new ArgumentNullException(nameof(dynamicComponent));
            case { AsElement: null, AsComponent: null } or { AsElement: not null, AsComponent: not null }:
                throw new InvalidOperationException(
                    $"Invalid dynamic component {dynamicComponent.GetType().Name}. This is probably due to a missing .OpenAs() call.");
        }

        if (dynamicComponent.AsElement != null)
        {
            builder.CloseElement();
        }
        else if (dynamicComponent.AsComponent != null)
        {
            builder.CloseComponent();
        }
    }

    public static RenderFragment? GetChildContent<TContext, TDynamic>(this TDynamic dynamicComponent,
        RenderFragment<TContext>? childContent)
        where TContext : IDynamicComponent where TDynamic : IDynamicParentComponent<TContext>, TContext
    {
        return GetChildContent<TContext, TDynamic>(dynamicComponent, childContent?.Invoke(dynamicComponent));
    }

    public static RenderFragment? GetChildContent<TContext, TDynamic>(this TDynamic dynamicComponent,
        RenderFragment? childContent)
        where TContext : IDynamicComponent where TDynamic : IDynamicParentComponent<TContext>, TContext
    {
        switch (dynamicComponent)
        {
            case null:
                throw new ArgumentNullException(nameof(dynamicComponent));
            case { AsElement: null, AsComponent: null } or { AsElement: not null, AsComponent: not null }:
                throw new InvalidOperationException(
                    $"Invalid dynamic component {dynamicComponent.GetType().Name}. This is probably due to a missing .OpenAs() call.");
        }

        return dynamicComponent._ != null ? dynamicComponent._.Invoke(dynamicComponent) : childContent;
    }

    public static void AddChildContentFor<TContext, TDynamic>(this RenderTreeBuilder builder, int sequence,
        TDynamic dynamicComponent, RenderFragment? childContent)
        where TContext : IDynamicComponent where TDynamic : IDynamicParentComponent<TContext>, TContext
    {
        AddContentFor(builder, sequence, dynamicComponent,
            GetChildContent<TContext, TDynamic>(dynamicComponent, childContent));
    }

#pragma warning disable ASP0006
    public static void AddContentFor(this RenderTreeBuilder builder, int sequence, IDynamicComponent dynamicComponent,
        RenderFragment? content)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        switch (dynamicComponent)
        {
            case null:
                throw new ArgumentNullException(nameof(dynamicComponent));
            case { AsElement: null, AsComponent: null } or { AsElement: not null, AsComponent: not null }:
                throw new InvalidOperationException(
                    $"Invalid dynamic component {dynamicComponent.GetType().Name}. This is probably due to a missing .OpenAs() call.");
        }

        if (content == null) return;

        if (dynamicComponent.AsElement != null)
        {
            builder.AddContent(sequence, content);
        }
        else if (dynamicComponent.AsComponent != null)
        {
            builder.AddAttribute(sequence, "ChildContent", content);
        }
    }
#pragma warning restore ASP0006

    public static ElementReference? TryProvideElementReference(this IDynamicComponent dynamicComponent)
    {
        if (dynamicComponent == null) throw new ArgumentNullException(nameof(dynamicComponent));

        return dynamicComponent.Component switch
        {
            IDynamicParentComponent component => component.TryProvideElementReference(),
            IElementReferenceProvider elementReferenceProvider => elementReferenceProvider.Element,
            _ => dynamicComponent.Element
        };
    }
}
