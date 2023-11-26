using System.Reflection;
using Ignis.Components.Extensions;
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

        serviceCollection.AddTransient<IFrameTracker, FrameTracker>();

        serviceCollection.TryAddScoped<IContentRegistry, ContentRegistry>();

        serviceCollection.TryAddSingleton<TimeProvider, TimeProviderImplementation>();

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

    private static RenderFragment? GetChildContent<T>(this T dynamicComponent, RenderFragment<T>? childContent)
        where T : IDynamicParentComponent<T>
    {
        return GetChildContent(dynamicComponent, childContent?.Invoke(dynamicComponent));
    }

    private static RenderFragment? GetChildContent<T>(this T dynamicComponent, RenderFragment? childContent)
        where T : IDynamicParentComponent<T>
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

    public static void AddChildContentFor<T>(this RenderTreeBuilder builder, int sequence, T dynamicComponent,
        RenderFragment? childContent) where T : IDynamicParentComponent<T>
    {
        AddContentForCore(builder, sequence, dynamicComponent, GetChildContent(dynamicComponent, childContent));
    }

    public static void AddChildContentFor<T>(this RenderTreeBuilder builder, int sequence, T dynamicComponent,
        RenderFragment<T>? childContent) where T : IDynamicParentComponent<T>
    {
        AddContentForCore(builder, sequence, dynamicComponent, GetChildContent(dynamicComponent, childContent));
    }

    public static void AddContentFor(this RenderTreeBuilder builder, int sequence, IDynamicComponent dynamicComponent,
        RenderFragment? content)
    {
        if (dynamicComponent.GetType().GetInterfaces().Any(i => i.IsGenericType &&
                                                                i.GetGenericTypeDefinition() ==
                                                                typeof(IDynamicParentComponent<>)))
            throw new InvalidOperationException(
                $"You cannot use {nameof(AddContentFor)} with a IDynamicParentComponent. Use {nameof(AddChildContentFor)} instead.");
        AddContentForCore(builder, sequence, dynamicComponent, content);
    }

#pragma warning disable ASP0006
    private static void AddContentForCore(this RenderTreeBuilder builder, int sequence,
        IDynamicComponent dynamicComponent, RenderFragment? content)
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
            IDynamicComponent component => component.TryProvideElementReference(),
            IElementReferenceProvider elementReferenceProvider => elementReferenceProvider.Element,
            _ => dynamicComponent.Element
        };
    }
}
