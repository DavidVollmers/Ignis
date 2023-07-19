using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ignis.Components;

public static class IgnisComponentExtensions
{
    public static IServiceCollection AddIgnisServices(this IServiceCollection serviceCollection)
    {
        if (serviceCollection is null) throw new ArgumentNullException(nameof(serviceCollection));

        serviceCollection.AddHttpContextAccessor();

        serviceCollection.TryAddSingleton<IServer, Server>();

        return serviceCollection;
    }

    public static void OpenAs(this RenderTreeBuilder builder, int sequence, IDynamicComponent dynamicComponent)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        switch (dynamicComponent)
        {
            case null:
                throw new ArgumentNullException(nameof(dynamicComponent));
            case { AsElement: not null, AsComponent: not null }:
                throw new InvalidOperationException(
                    $"Cannot specify both AsElement and AsComponent for {dynamicComponent.GetType().Name}.");
            case { AsElement: null, AsComponent: null }:
                throw new InvalidOperationException(
                    $"Must specify either AsElement or AsComponent for {dynamicComponent.GetType().Name}.");
        }

        if (dynamicComponent.AsElement != null)
        {
#pragma warning disable ASP0006
            builder.OpenElement(sequence, dynamicComponent.AsElement);
#pragma warning restore ASP0006
        }
        else if (dynamicComponent.AsComponent != null)
        {
            if (dynamicComponent.AsComponent.GetInterface(nameof(IComponent)) == null)
            {
                throw new InvalidOperationException(
                    $"Invalid component type {dynamicComponent.AsComponent.Name}. Must implement {nameof(IComponent)}.");
            }

#pragma warning disable ASP0006
            builder.OpenComponent(sequence, dynamicComponent.AsComponent);
#pragma warning restore ASP0006
        }
    }

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

    public static void AddChildContentFor<TContext, TDynamic>(this RenderTreeBuilder builder, int sequence,
        TDynamic dynamicComponent)
        where TContext : IDynamicComponent where TDynamic : IDynamicParentComponent<TContext>, TContext
    {
        AddContentFor(builder, sequence, dynamicComponent, dynamicComponent.ChildContent?.Invoke(dynamicComponent));
    }

    public static void AddChildContentFor(this RenderTreeBuilder builder, int sequence,
        IDynamicParentComponent dynamicComponent)
    {
        AddContentFor(builder, sequence, dynamicComponent, dynamicComponent.ChildContent?.Invoke(dynamicComponent));
    }

    public static void AddContentFor(this RenderTreeBuilder builder, int sequence, IDynamicComponent dynamicComponent,
        RenderFragment? content, string childContentName = "ChildContent")
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
#pragma warning disable ASP0006
            builder.AddContent(sequence, content);
#pragma warning restore ASP0006
        }
        else if (dynamicComponent.AsComponent != null)
        {
#pragma warning disable ASP0006
            builder.AddAttribute(sequence, childContentName, content);
#pragma warning restore ASP0006
        }
    }

    public static void AddReferenceCaptureFor(this RenderTreeBuilder builder, int sequence,
        IDynamicComponent dynamicComponent, Action<ElementReference> elementCapture,
        Action<object> componentCapture)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        if (elementCapture == null) throw new ArgumentNullException(nameof(elementCapture));
        if (componentCapture == null) throw new ArgumentNullException(nameof(componentCapture));
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
#pragma warning disable ASP0006
            builder.AddElementReferenceCapture(sequence, elementCapture);
#pragma warning restore ASP0006
        }
        else if (dynamicComponent.AsComponent != null)
        {
#pragma warning disable ASP0006
            builder.AddComponentReferenceCapture(sequence, componentCapture);
#pragma warning restore ASP0006
        }
    }
}
